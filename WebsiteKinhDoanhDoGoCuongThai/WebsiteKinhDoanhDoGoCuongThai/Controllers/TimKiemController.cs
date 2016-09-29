using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteKinhDoanhDoGoCuongThai.Models; 
using PagedList.Mvc;
using PagedList;
namespace WebsiteKinhDoanhDoGoCuongThai.Controllers
{
    public class TimKiemController : Controller
    {

        QLDoGoDataContext db = new QLDoGoDataContext();
        // GET: TimKiem

        [HttpPost, ValidateInput(false)]
        public ActionResult KetQuaTimKiem(FormCollection frmCollection, int? page)
        {
            string chuoiTimKiem = frmCollection["txtTimKiem"].ToString(); 
            List<HANGHOA> lstHangHoa = db.HANGHOAs.Where(n => n.TenMatHang.Contains(chuoiTimKiem)).ToList();
            //Phan trang
            int pageNumber = (page ?? 1);
            int pageSize = 9; 

            ViewBag.chuoiTimKiem = chuoiTimKiem; 
            //neu ket qua ko tim thay hang
            if (lstHangHoa.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy hàng hóa nào.";
                return View(db.HANGHOAs.OrderBy(n => n.TenMatHang).ToPagedList(pageNumber, pageSize));
            }
            ViewBag.ThongBao = chuoiTimKiem;  
            return View(lstHangHoa.OrderBy(n => n.TenMatHang).ToPagedList(pageNumber, pageSize));
             
        }

        [HttpGet, ValidateInput(false)]
        public ActionResult KetQuaTimKiem(int? page, string chuoiTimKiem=" ")
        {
            ViewBag.chuoiTimKiem = chuoiTimKiem;
            List<HANGHOA> lstHangHoa = db.HANGHOAs.Where(n => n.TenMatHang.Contains(chuoiTimKiem)).ToList();

            //Phan trang
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            
            //neu ket qua ko tim thay hang
            if (lstHangHoa.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy hàng hóa nào.";
                return View(db.HANGHOAs.OrderBy(n => n.TenMatHang).ToPagedList(pageNumber, pageSize));
            }

            ViewBag.ThongBao = chuoiTimKiem; 
            return View(lstHangHoa.OrderBy(n => n.TenMatHang).ToPagedList(pageNumber, pageSize));
          
        }
    }
}