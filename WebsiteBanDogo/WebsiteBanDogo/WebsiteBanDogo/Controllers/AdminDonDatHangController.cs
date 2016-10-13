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
    public class AdminDonDatHangController : Controller
    {
        // GET: AdminDonDatHang

        QLDoGoDataContext db = new QLDoGoDataContext();

        public ActionResult Index()
        {
            return View();
        }

        //SỬA ĐĐH
        [HttpGet]
        public ActionResult SuaDonDatHang(int id)
        {
            if (Session["TKAdmin"] != null)
            {
                // ViewBag.MALOAIHANG = new SelectList(db.LOAIHANGs.ToList().OrderBy(n => n.TENLOAIHANG), "MALOAIHANG", "TENLOAIHANG");
                DONDATHANG ddh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonDatHang == id);
                ViewBag.NguoiDatHang = db.KHACHHANGs.SingleOrDefault(n => n.MaKhachHang == ddh.MaKhachHang).TenKhachHang;
                if (ddh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ddh);
            }
            else
                return RedirectToAction("DangNhap", "Admin");
        }

        [HttpPost]
        //kiểm tra giá trị đúng sai nếu sai giá trị nó sẽ báo lỗi sài trong đăng ký
        [ValidateInput(false)]
        public ActionResult SuaDonDatHang(DONDATHANG ddh, FormCollection collection)
        {
            DONDATHANG ddhh = db.DONDATHANGs.Where(x => x.MaDonDatHang == ddh.MaDonDatHang).FirstOrDefault();

            var tennguoinhan = collection["HOTENNGUOINHAN"];

            if (ModelState.IsValid)
            {
                ddhh.HoTenNguoiNhan = tennguoinhan;
                UpdateModel(ddhh);
                db.SubmitChanges();

            } 
            return RedirectToAction("Index", "Admin"); 
        }
    }
}