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
    public class AdminKhachHangController : Controller
    {
        QLDoGoDataContext db = new QLDoGoDataContext();
        // GET: AdminKhachHang
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult dsKhachHang(int? page)
        {
            if (Session["TKAdmin"] != null)
            {
                //Tạo biến Quy định sô sản phẩm trên 1 trang
                int pagesize = 9;
                //Tạo biến số sang
                int pagenum = (page ?? 1);
                List<KHACHHANG> lstKhachHang = db.KHACHHANGs.ToList(); 
                return View(lstKhachHang.ToPagedList(pagenum, pagesize));
            }
            else
            {
                return RedirectToAction("DangNhap", "Admin");
            }   
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [ChildActionOnly]​
        public ActionResult dsthichsanpham(int id)
        {
            var lst = (from khachhang in db.KHACHHANGs
                       from hang in db.DSHangYeuThiches
                       where (khachhang.MaKhachHang == hang.MaKhachHang) && (hang.MaKhachHang == id)
                       select new { 
                             tenkhachhang = khachhang.TenKhachHang,
                             soluonghang = hang.MaMatHang.Count()
                       }).ToList();
            var ls = lst.Count();
            return PartialView(lst);
        }
    }
}