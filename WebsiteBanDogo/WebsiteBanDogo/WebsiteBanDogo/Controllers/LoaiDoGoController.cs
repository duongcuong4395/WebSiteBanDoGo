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
            return PartialView(db.LOAIHANGs.ToList());
        }

        public ViewResult SanPhamTheoLoaiDoGo(string id, int? page)
        { 
            //Tạo biến Quy định sô sản phẩm trên 1 trang
            int pagesize = 12;
            //Tạo biến số sang
            int pagenum = (page ?? 1);

            LOAIHANG lh = db.LOAIHANGs.SingleOrDefault(n => n.MaLoaiHang == id);
            ViewBag.TenLoai = lh.TenLoaiHang;
            // kiểm tra loại hàng tồn tại
            if (lh == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<HANGHOA> lstHangHoa = db.HANGHOAs.Where(n => n.MaLoaiHang == id).OrderByDescending(n => n.DonGia).ToList();
            if (lstHangHoa.Count == 0)
            {
                ViewBag.HANGHOA = "Không tìm thấy loại thàng nào";
            }
            return View(lstHangHoa.ToPagedList(pagenum, pagesize));
        }  
    }
}