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

        QLDoGoDataContext db = new QLDoGoDataContext();
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
                return RedirectToAction("DangNhap", "Admin");
            }   
        }

        [HttpGet]
        public ActionResult SuaSanPham(string id)
        {
            if (Session["TKAdmin"] != null)
            {
                HANGHOA hanghoa = db.HANGHOAs.SingleOrDefault(n => n.MaMatHang == id);
                if (hanghoa == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                ViewBag.MaLoaiHang = new SelectList(db.LOAIHANGs.ToList().OrderBy(n => n.TenLoaiHang), "MaLoaiHang", "TenLoaiHang");
                return View(hanghoa);
            }
            else
            {
                return RedirectToAction("DangNhap", "Admin");
            }
        }

        [HttpPost]
        //kiểm tra giá trị đúng sai nếu sai giá trị nó sẽ báo lỗi sài
        [ValidateInput(false)]
        public ActionResult SuaSanPham(HANGHOA hanghoa, HttpPostedFileBase fileUpload, FormCollection frmCollection)
        {
            HANGHOA hanghoamoi = db.HANGHOAs.Where(n => n.MaMatHang == hanghoa.MaMatHang).FirstOrDefault();

            ViewBag.MaLoaiHang = new SelectList(db.LOAIHANGs.ToList().OrderBy(n => n.TenLoaiHang), "MaLoaiHang", "TenLoaiHang");

            var MaLoaiHang = frmCollection["MaLoaiHang"];
            var tensanpham = frmCollection["TenSanPham"]; 
            var GiaSanPham = frmCollection["GiaSanPham"];
            var KhuyenMai = frmCollection["KhuyenMai"];
            var KichThuoc = frmCollection["KichThuoc"];
            var LoaiGo = frmCollection["LoaiGo"];
            var BaoHanh = frmCollection["BaoHanh"];
            var MoTa = frmCollection["MoTa"];
            var linkThanhToan = frmCollection["LINKTHANHTOANOL"];

            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    hanghoamoi.MaLoaiHang = MaLoaiHang; 
                    hanghoamoi.TenMatHang = tensanpham;
                    hanghoamoi.GiaMoi = Decimal.Parse(GiaSanPham);
                    hanghoamoi.KhuyenMai = Decimal.Parse(KhuyenMai);
                    hanghoamoi.KichThuoc = KichThuoc;
                    hanghoamoi.LoaiGo = LoaiGo;
                    hanghoamoi.BaoHanh = BaoHanh;
                    hanghoamoi.MoTa = MoTa;
                    hanghoamoi.BanChay = true;
                    hanghoamoi.LinkThanhToan = linkThanhToan;
                    //Lưu tên file
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    // Lưu Đường dẫn của file
                    var path = Path.Combine(Server.MapPath("/Pictures/HangHoa"), fileName);
                    //Kiểm tra hình ảnh đã tồn tại chưa
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Ảnh đã bị trùng";
                    }
                    else
                    { 
                        //Lưu hình ảnh
                        fileUpload.SaveAs(path);
                    }
                    hanghoamoi.HinhAnh = fileName;

                    UpdateModel(hanghoamoi);
                    db.SubmitChanges();
                }
            } 
            return RedirectToAction("HienSanPham");
        }


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
                return RedirectToAction("DangNhap", "Admin");
            }  
        }
    }
}