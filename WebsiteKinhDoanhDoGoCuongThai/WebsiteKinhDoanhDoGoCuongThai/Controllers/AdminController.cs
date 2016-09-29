using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteKinhDoanhDoGoCuongThai.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebsiteKinhDoanhDoGoCuongThai.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        QLDoGoDataContext db = new QLDoGoDataContext();

        public ActionResult Index()
        {
            if (Session["TKAdmin"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("dangnhap", "Admin");
            } 
        }

        //Thực hiện form login Admin
        [HttpGet]
        public ActionResult dangnhap()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult dangnhap(FormCollection frmcollection)
        {
            var TenDangNhapAdmin = frmcollection["TenDangNhapAdmin"];
            var MatKhauAdmin = frmcollection["MatKhauAdmin"];

            ADMIN ad = db.ADMINs.SingleOrDefault(n => n.TaiKhoan == TenDangNhapAdmin && n.MatKhau == MatKhauAdmin); 
            if (ad != null)
            {
                // ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                Session["ADMIN"] = ad;
                Session["TKAdmin"] = ad.TaiKhoan;
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
            Session["TKAdmin"] = null;
            return RedirectToAction("Login", "Admin");
        }
    }
}