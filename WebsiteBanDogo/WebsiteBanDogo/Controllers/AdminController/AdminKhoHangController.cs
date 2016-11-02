using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebsiteBanDogo.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebsiteBanDogo.Controllers
{
    public class AdminKhoHangController : Controller
    {
        // GET: KhoHangAdmin

        QLDoGoCuongThaiDataContext db = new QLDoGoCuongThaiDataContext();
        // Quản lý hàng hóa trong khho 
        // Gồm
        // Kho Có thể chứa được bao nhiêu sản phẩm(Max = ?).
        // sản phẩm.(Số lượng tồn, giá, tên sản phẩm, khuyến mãi.)
        // Loại sản phẩm.(Tên loại, số lượng loại sản phẩm.)

        public ActionResult Index()
        {
            return View();
        }

        //Quản lý sản phẩm.
        #region Thao tác trên sản phẩm
        //Lấy sản phẩm
        private List<HANGHOA> laySanPham()
        {
            return db.HANGHOAs.OrderByDescending(a => a.MaMatHang).ToList();
        }

        
        //Hiện sản phẩm
        public ActionResult HienSanPham(int? page)
        {
            if (Session["TKAdmin"] != null)
            {
                //Tạo biến Quy định sô sản phẩm trên 1 trang
                int pagesize = 9;
                //Tạo biến số sang
                int pagenum = (page ?? 1);

                // Lay ra 6 san pham
                var sanpham = laySanPham();
                return View(sanpham.ToPagedList(pagenum, pagesize));
            }
            else
            {
                return RedirectToAction("DangNhap", "AdminQL");
            }   
        }

        [HttpGet]
        public ActionResult themsanpham()
        {
            if (Session["TKAdmin"] != null)
            {
                if (bool.Parse(Session["PhanQuyenAdmin"].ToString()) == true)
                {
                    ViewBag.MaLoaiHang = new SelectList(db.LOAIHANGs.ToList().OrderBy(n => n.TenLoaiHang), "MaLoaiHang", "TenLoaiHang");
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "AdminQL");
                } 
            }
            else
            {
                return RedirectToAction("DangNhap", "AdminQL");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult themsanpham(HANGHOA hanghoa, HttpPostedFileBase fileUpload)
        {
            try
            {
                ViewBag.MaLoaiHang = new SelectList(db.LOAIHANGs.ToList().OrderBy(n => n.TenLoaiHang), "MaLoaiHang", "TenLoaiHang");

                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var filename = Path.GetFileName(fileUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/Pictures/HangHoa"), filename);
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                        }
                        else
                        {
                            fileUpload.SaveAs(path);
                        }
                        hanghoa.HinhAnh = filename;
                        db.HANGHOAs.InsertOnSubmit(hanghoa);
                        db.SubmitChanges();
                    }

                }
                return RedirectToAction("HienSanPham", "AdminKhoHang");
            }
            catch (Exception error)
            {
                return RedirectToAction("HienSanPham", "AdminKhoHang");
            } 
        }

        [HttpGet]
        public ActionResult SuaSanPham(string MaMatHang)
        {
            if (Session["TKAdmin"] != null)
            {
                if (bool.Parse(Session["PhanQuyenAdmin"].ToString()) == true)
                {
                    HANGHOA hanghoa = db.HANGHOAs.Where(n => n.MaMatHang == MaMatHang).FirstOrDefault();
                    ViewBag.MaMatHang = hanghoa.MaMatHang;
                    if (hanghoa == null)
                    {
                        Response.StatusCode = 404;
                        return null;
                    }
                    ViewBag.MaLoaiHang = new SelectList(db.LOAIHANGs.ToList().OrderBy(n => n.TenLoaiHang), "MaLoaiHang", "TenLoaiHang", hanghoa.MaLoaiHang);

                    return View(hanghoa);
                }
                else
                {
                    return RedirectToAction("Index", "AdminQL");
                }
            }
            else
            {
                return RedirectToAction("DangNhap", "AdminQL");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SuaSanPham(HANGHOA hanghoa, HttpPostedFileBase fileUpload, FormCollection frm)
        {
            try
            {
                HANGHOA hh = db.HANGHOAs.Where(x => x.MaMatHang == hanghoa.MaMatHang).FirstOrDefault(); 
                ViewBag.MaLoaiHang = new SelectList(db.LOAIHANGs.ToList().OrderBy(n => n.TenLoaiHang), "MaLoaiHang", "TenLoaiHang");
                string tam = frm["NOIDUNG"];
                var TenMatHang = frm["TenMatHang"];
                var KichThuoc = frm["KichThuoc"];
                var LoaiGo = frm["LoaiGo"];
                if (tam == "")
                {
                    ViewData["LOI1"] = "Nội dung không được bỏ trống";
                    return this.SuaSanPham(hanghoa.MaMatHang);
                }
                if (fileUpload == null)
                {
                    ViewBag.ThongBao = "Chọn ảnh bìa.";
                    UpdateModel(hh);
                    db.SubmitChanges();
                    return RedirectToAction("Index", "AdminQL");
                }
                else
                { 
                    if (ModelState.IsValid)
                    {
                        var filename = Path.GetFileName(fileUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/Pictures/HangHoa"), filename);
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                        }
                        else
                        {
                            fileUpload.SaveAs(path);
                        } 
                        hh.HinhAnh = filename;
                        hh.TenMatHang = TenMatHang;
                        hh.KichThuoc = KichThuoc;
                        hh.LoaiGo = LoaiGo;
                        UpdateModel(hh); 
                        db.SubmitChanges();
                        ViewBag.MaMatHang = hh.MaMatHang;
                        return RedirectToAction("Index", "AdminQL");
                    }
                }
                return RedirectToAction("HienSanPham", "AdminKhoHang");
            }
            catch(Exception error)
            {
                return RedirectToAction("Index", "AdminQL");
            } 
        } 
         

        [HttpGet]
        public ActionResult XoaSanPham(string id)
        {
            if (Session["TKAdmin"] != null)
            {
                if (bool.Parse(Session["PhanQuyenAdmin"].ToString()) == true)
                {
                    HANGHOA hanghoa = db.HANGHOAs.Where(n => n.MaMatHang == id).FirstOrDefault();
                    ViewBag.masach = hanghoa.MaMatHang;
                    if (hanghoa == null)
                    {
                        Response.StatusCode = 404;
                        return null;
                    }
                    return View(hanghoa);
                }
                return RedirectToAction("Index", "AdminQL");
            }
            else
            {
                return RedirectToAction("DangNhap", "AdminQL");
            }
        }

        [HttpPost, ActionName("XoaSanPham")]
        public ActionResult XacNhanXoaSanPham(string id)
        {
            try
            {
                HANGHOA hanghoa = db.HANGHOAs.Where(n => n.MaMatHang == id).FirstOrDefault();
                ViewBag.MaSach = hanghoa.MaMatHang;
                if (hanghoa == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                db.HANGHOAs.DeleteOnSubmit(hanghoa);
                db.SubmitChanges();
                return RedirectToAction("HienSanPham", "AdminKhoHang");
            }
            catch (Exception error)
            {
                return RedirectToAction("HienSanPham", "AdminKhoHang");
            }
        }
       #endregion

        private List<LOAIHANG> layLoaiHang()
        {
            return db.LOAIHANGs.OrderByDescending(a => a.MaLoaiHang).ToList();
        }

        public ActionResult HienLoaiSanPham(int? page)
        {
            if (Session["TKAdmin"] != null)
            {
                //Tạo biến Quy định sô sản phẩm trên 1 trang
                int pagesize = 9;
                //Tạo biến số sang
                int pagenum = (page ?? 1);

                // Lay ra 6 san pham
                var loaiSanPham = layLoaiHang();
                return View(loaiSanPham.ToPagedList(pagenum, pagesize));
            }
            else
            {
                return RedirectToAction("DangNhap", "AdminQL");
            }  
        }

        [HttpGet]
        public ActionResult SPUaChuon( )
        {
            if (Session["TKAdmin"] != null)
            { 
                var sp = db.DS_SPUaChuon();
                return PartialView(sp); 
            }
            else
            {
                return RedirectToAction("DangNhap", "AdminQL");
            }
        }
    }
}