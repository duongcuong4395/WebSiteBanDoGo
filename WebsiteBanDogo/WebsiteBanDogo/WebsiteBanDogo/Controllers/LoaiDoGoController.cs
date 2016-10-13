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
    public class LoaiDoGoController : Controller
    {
        // GET: LoaiDoGo

        QLDoGoDataContext db = new QLDoGoDataContext();

        //Danh sách loại đồ gỗ 
        public ActionResult LoaiDoGoPartial()
        { 
            var slhh = db.DanhSach_HangHoa();
            return PartialView(slhh);
        }

        private int soHangTheoLoai(string MaLoaiHang)
        {
            var soluong = db.HANGHOAs.SingleOrDefault(n => n.MaLoaiHang == MaLoaiHang).MaMatHang.Count();
            return soluong;
        }

        public ViewResult SanPhamTheoLoaiDoGo(string MaLoaiHang, int? page)
        {
            //Tạo biến Quy định sô sản phẩm trên 1 trang
            int pagesize = 12;
            //Tạo biến số sang
            int pagenum = (page ?? 1);

            LOAIHANG lh = db.LOAIHANGs.SingleOrDefault(n => n.MaLoaiHang == MaLoaiHang); 
            // kiểm tra loại hàng tồn tại
            if (lh == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<HANGHOA> lstHangHoa = db.HANGHOAs.Where(n => n.MaLoaiHang == MaLoaiHang).OrderByDescending(n => n.DonGia).ToList();
            if (lstHangHoa.Count == 0)
            {
                ViewBag.HANGHOA = "Không tìm thấy loại thàng nào";
            }
            ViewBag.TenLoai = lh.TenLoaiHang;
            return View(lstHangHoa.ToPagedList(pagenum, pagesize));
        }
    }
}