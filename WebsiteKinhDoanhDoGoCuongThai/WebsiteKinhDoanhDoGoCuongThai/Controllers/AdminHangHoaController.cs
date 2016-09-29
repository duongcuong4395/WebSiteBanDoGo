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
    public class AdminHangHoaController : Controller
    {
        // GET: HangHoaAdmin

        QLDoGoDataContext db = new QLDoGoDataContext();

        public ActionResult Index()
        {
            return View();
        }

        //Amin sẽ quản lí hàng hóa từ Các action ở đây

        //hiển thị hết tất cả sản phẩm
        public ActionResult HangHoaAdmin(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.HANGHOAs.ToList().OrderBy(n => n.MaLoaiHang).ToPagedList(pageNumber, pageSize));
        }
         
        //Thêm một sản phẩm
        [HttpGet]
        public ActionResult ThemSanPhamAdmin()
        { 
            ViewBag.MaLoaiHang = new SelectList(db.LOAIHANGs.ToList().OrderBy(n => n.TenLoaiHang), "MaLoaiHang", "TenLoaiHang");

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemSanPhamAdmin(HANGHOA hanghoa, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaLoaiHang = new SelectList(db.LOAIHANGs.ToList().OrderBy(n => n.TenLoaiHang), "MaLoaiHang", "TenLoaiHang");

            //Kiểm tra đường dẫn file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "ảnh bìa bị trùng";
                return View();
            }
            //Thêm vào cơ sở dữ liệu
            else
            { 
                if (ModelState.IsValid)
                {
                    if (db.HANGHOAs.Where(m => m.MaMatHang == hanghoa.MaMatHang).FirstOrDefault() != null)
                    {
                        ViewData["txtMaMatHang"] = "Mã sản phẩm đã tồn tại, xin nhập mã khác !";
                        return View(hanghoa);
                    }
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Pictures/HangHoa/"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    hanghoa.HinhAnh = fileName;
                    db.HANGHOAs.InsertOnSubmit(hanghoa);
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("HangHoaAdmin", "AdminHangHoa");
        }

        public ViewResult XemChiTietAdmin(string mamathang = " ")
        {
            HANGHOA hang = db.HANGHOAs.SingleOrDefault(n => n.MaMatHang == mamathang);
            if (hang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hang);
        }
    }
}