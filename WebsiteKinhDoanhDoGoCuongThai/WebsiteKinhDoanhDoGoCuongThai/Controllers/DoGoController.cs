using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteKinhDoanhDoGoCuongThai.Models;

namespace WebsiteKinhDoanhDoGoCuongThai.Controllers
{
    public class DoGoController : Controller
    {
        QLDoGoDataContext db = new QLDoGoDataContext();
        public ViewResult XemChiTiet(string mamathang=" ")
        {
            HANGHOA hang = db.HANGHOAs.SingleOrDefault(n => n.MaMatHang == mamathang);
            if (hang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hang);
        }
         
        public ViewResult SanPhamTheoLoaiDoGo(string maloaihang = " ")
        {
            LOAIHANG lh = db.LOAIHANGs.SingleOrDefault(n => n.MaLoaiHang == maloaihang);
            ViewBag.TenLoai = lh.TenLoaiHang;
            // kiểm tra loại hàng tồn tại
            if (lh == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<HANGHOA> lstHangHoa = db.HANGHOAs.Where(n => n.MaLoaiHang == maloaihang).ToList();
            if (lstHangHoa.Count == 0)
            {
                ViewBag.HANGHOA = "Không tìm thấy loại hàng nào";
            }
            return View(lstHangHoa);
        } 
    }
}