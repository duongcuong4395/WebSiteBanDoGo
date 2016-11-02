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

        QLDoGoCuongThaiDataContext db = new QLDoGoCuongThaiDataContext();

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
                if (bool.Parse(Session["PhanQuyenAdmin"].ToString()) == true)
                {
                    // ViewBag.MALOAIHANG = new SelectList(db.LOAIHANGs.ToList().OrderBy(n => n.TENLOAIHANG), "MALOAIHANG", "TENLOAIHANG");
                    DONDATHANG ddh = db.DONDATHANGs.Where(n => n.MaDonDatHang == id).FirstOrDefault();
                    ViewBag.NguoiDatHang = db.KHACHHANGs.Where(n => n.MaKhachHang == ddh.MaKhachHang).FirstOrDefault().TenKhachHang;
                    if (ddh == null)
                    {
                        Response.StatusCode = 404;
                        return null;
                    }
                    return View(ddh);
                }
                else
                {
                    return RedirectToAction("Index", "AdminQL");
                }
            }
            else
                return RedirectToAction("DangNhap", "AdminQL");
        }

        [HttpPost]
        //kiểm tra giá trị đúng sai nếu sai giá trị nó sẽ báo lỗi sài trong đăng ký
        [AllowAnonymous]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SuaDonDatHang(DONDATHANG ddh, FormCollection collection)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    DONDATHANG ddhh = db.DONDATHANGs.Where(x => x.MaDonDatHang == ddh.MaDonDatHang).FirstOrDefault(); 
                    var tennguoinhan = collection["HOTENNGUOINHAN"]; 
                    if (ModelState.IsValid)
                    {
                        ddhh.HoTenNguoiNhan = tennguoinhan;
                        UpdateModel(ddhh);
                        db.SubmitChanges(); 
                    }
                } 
                return RedirectToAction("Index", "AdminQL");
            }
            catch (Exception error)
            {
                return RedirectToAction("Index", "AdminQL");
            } 
        }
    }
}