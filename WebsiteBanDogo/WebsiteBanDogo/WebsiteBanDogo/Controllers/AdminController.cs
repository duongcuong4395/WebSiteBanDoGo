using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebsiteBanDogo.Models;
using PagedList;
using PagedList.Mvc;

namespace WebsiteBanDogo.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        QLDoGoDataContext db = new QLDoGoDataContext();

        public ActionResult Index(int? page)
        {
            if (Session["TKAdmin"] != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 5;
                return View(db.DONDATHANGs.ToList().OrderBy(n => n.MaDonDatHang).ToPagedList(pageNumber, pageSize)); 
            }
            else
            {
                return RedirectToAction("DangNhap", "Admin");
            }  
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection frmcollection)
        {
            var TenDangNhapAdmin = frmcollection["TenDangNhapAdmin"];
            var MatKhauAdmin = frmcollection["MatKhauAdmin"];

            ADMIN ad = db.ADMINs.SingleOrDefault(n => n.TaiKhoan == TenDangNhapAdmin && n.MatKhau == MatKhauAdmin);
            if (ad != null)
            {
                // ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                Session["ADMIN"] = ad;
                Session["TKAdmin"] = ad.TaiKhoan;
                Session["HoTenAdmin"] = ad.HoTen;
                Session["MaAdmin"] = ad.MaAdmin;
                Session["PhanQuyenAdmin"] = ad.PhanQuyen;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng.";
            }
            return View(); 
        }

        public ActionResult DangXuat()
        {
            Session.Abandon();
            return RedirectToAction("DangNhap", "Admin");
        }
    }
}