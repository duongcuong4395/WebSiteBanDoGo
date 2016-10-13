using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanDogo.Models;
 
using System.Web.Optimization;
using System.Web.Routing;


using PagedList;
using PagedList.Mvc;

namespace WebsiteBanDogo.Controllers
{
    public class DoGoStoreController : Controller 
    {
        // GET: DoGoStore

        QLDoGoDataContext db = new QLDoGoDataContext();

        private List<HANGHOA> layHangHoa()
        {
            return db.HANGHOAs.OrderByDescending(n => n.DonGia).ToList();
        } 

        public ActionResult Index(int? page)
        {
            
            if (db.NguoiXems.Where(n => n.NgayXem == null).Take(1) == null)
            { 
                NguoiXem nx = new NguoiXem();
                nx.SoNguoiXem = 1;
                nx.NgayXem = DateTime.Parse(String.Format("{0:yyyy/MM/dd}", DateTime.Now));
                db.NguoiXems.InsertOnSubmit(nx);
                db.SubmitChanges();
            }
            else
            { 
                string ngayhientai = String.Format("{0:yyyy/MM/dd}", DateTime.Now);
                DateTime ngay = DateTime.Parse(ngayhientai);
                NguoiXem nx = db.NguoiXems.SingleOrDefault(n => n.NgayXem == ngay);
                if (nx != null)
                {
                    var songuoixemhientai = nx.SoNguoiXem;
                    int snht = int.Parse(songuoixemhientai.ToString());
                    int snx = snht + int.Parse(Session["online"].ToString());
                    nx.SoNguoiXem = snx;
                    nx.NgayXem = nx.NgayXem;
                    UpdateModel(nx);
                    db.SubmitChanges();
                }
                else
                {
                    NguoiXem nguoixem = new NguoiXem();
                    nguoixem.SoNguoiXem = 1;
                    nguoixem.NgayXem = DateTime.Parse(String.Format("{0:yyyy/MM/dd}", DateTime.Now));
                    db.NguoiXems.InsertOnSubmit(nguoixem);
                    db.SubmitChanges();
                } 
            }
             

            //Tạo biến Quy định sô sản phẩm trên 1 trang
            int pagesize = 9;
            //Tạo biến số sang
            int pagenum = (page ?? 1);
            var sanpham = layHangHoa();
            return View(sanpham.ToPagedList(pagenum, pagesize));
        }

        public ActionResult hienSanPham()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LienHeCuaHang()
        {
            return View(db.LIENHEs.ToList());
        }

        [HttpPost] 
        public ActionResult LienHeCuaHang(FormCollection frmCollection, HOMTHU homthu)
        {
            // Gán các giá tị người dùng nhập liệu cho các biến 
            var tenNguoiGui = frmCollection["TenNguoiGui"];
            var email = frmCollection["Email"];
            var ChuDe = frmCollection["ChuDe"];
            var NoiDung = frmCollection["NoiDung"];
            if (String.IsNullOrEmpty(NoiDung))
            {
                ViewData["txtNoiDung"] = "Hãy góp ý ở phần nội dung.";
            }

            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh) 
                homthu.TenNguoiGui = tenNguoiGui;
                homthu.Email = email;
                homthu.ChuDe = ChuDe;
                homthu.NoiDung = NoiDung;
                homthu.NgayGui = DateTime.Now;
                db.HOMTHUs.InsertOnSubmit(homthu);
                db.SubmitChanges();
            }
            return this.LienHeCuaHang();
        }

        public ActionResult GioiThieuCuaHang()
        {
            ViewBag.hinh1 = db.GIOITHIEUs.FirstOrDefault(n => n.MaGioiThieu == 1).HinhAnhGioiThieu;
            ViewBag.hinh2 = db.GIOITHIEUs.FirstOrDefault(n => n.MaGioiThieu == 2).HinhAnhGioiThieu;
            ViewBag.hinh3 = db.GIOITHIEUs.FirstOrDefault(n => n.MaGioiThieu == 3).HinhAnhGioiThieu;

            ViewBag.noidung1 = db.GIOITHIEUs.FirstOrDefault(n => n.MaGioiThieu == 1).NoiDung;
            ViewBag.noidung2 = db.GIOITHIEUs.FirstOrDefault(n => n.MaGioiThieu == 2).NoiDung;
            ViewBag.noidung3 = db.GIOITHIEUs.FirstOrDefault(n => n.MaGioiThieu == 3).NoiDung;

            ViewBag.tieude1 = db.GIOITHIEUs.FirstOrDefault(n => n.MaGioiThieu == 1).TieuDe;
            ViewBag.tieude2 = db.GIOITHIEUs.FirstOrDefault(n => n.MaGioiThieu == 2).TieuDe;
            ViewBag.tieude3 = db.GIOITHIEUs.FirstOrDefault(n => n.MaGioiThieu == 3).TieuDe;

            ViewBag.hotline1 = db.GIOITHIEUs.FirstOrDefault(n => n.MaGioiThieu == 1).Hotline;
            ViewBag.hotline2 = db.GIOITHIEUs.FirstOrDefault(n => n.MaGioiThieu == 2).Hotline;
            ViewBag.hotline3 = db.GIOITHIEUs.FirstOrDefault(n => n.MaGioiThieu == 3).Hotline;
            return View();
        } 

        public ActionResult layHangBanChay()
        {
            return PartialView();
        }
        public ActionResult layHangBanChayitem()
        {
            return PartialView(db.HANGHOAs.Where(n => n.BanChay == true).OrderByDescending(a => a.MaMatHang).Take(6).ToList());
        }

        public ActionResult ThemSPYeuThich(string MaMatHang, string strURL)
        {
            if (Session["Taikhoan"] != null)
            { 
                int makh = db.KHACHHANGs.SingleOrDefault(n => n.TenKhachHang == (Session["TenKhachHang"]).ToString()).MaKhachHang;
                DSHangYeuThich dsYeuThich = db.DSHangYeuThiches.SingleOrDefault(n => n.MaMatHang == MaMatHang && n.MaKhachHang == makh);
                if (dsYeuThich == null)
                {
                    DSHangYeuThich ds = new DSHangYeuThich();
                    ds.MaMatHang = MaMatHang;
                    ds.MaKhachHang = makh;
                    db.DSHangYeuThiches.InsertOnSubmit(ds);
                    db.SubmitChanges();
                    ViewBag.ThongBao = "Đã thêm sản phẩm vào mục yêu thích.";
                }
                else
                {
                    ViewBag.ThongBao = "Sản phẩm này đã có trong danh mục yêu thích.";
                }
                return Redirect(strURL);
            }
            else
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
        }

        public ActionResult XoaSPYeuThich(string MaMatHang, string strURL)
        {
            if (Session["Taikhoan"] != null)
            {
                int makh = db.KHACHHANGs.SingleOrDefault(n => n.TenKhachHang == (Session["TenKhachHang"]).ToString()).MaKhachHang;
                DSHangYeuThich dsYeuThich = db.DSHangYeuThiches.SingleOrDefault(n => n.MaMatHang == MaMatHang && n.MaKhachHang == makh);
                string tenhang = db.HANGHOAs.SingleOrDefault(n => n.MaMatHang == MaMatHang).TenMatHang;

                if (dsYeuThich == null)
                { 
                    ViewBag.ThongBao = "Sản phẩm này không nằm trong danh sách thích.";
                }
                else
                {
                    ViewBag.ThongBao = "Đã xóa sản phẩm: " + tenhang + " trong danh mục yêu thích.";
                    db.DSHangYeuThiches.DeleteOnSubmit(dsYeuThich);
                    db.SubmitChanges(); 
                }
                return Redirect(strURL);
            }
            else
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
        }

        public ActionResult DShangYeuThich(int? page)
        { 
            //Tạo biến Quy định sô sản phẩm trên 1 trang
            int pagesize = 9;
            //Tạo biến số sang
            int pagenum = (page ?? 1);

            int makh = db.KHACHHANGs.SingleOrDefault(n => n.TenKhachHang == (Session["TenKhachHang"]).ToString()).MaKhachHang;
            var layhang = from dsthich in db.DSHangYeuThiches
                          join hanghoa in db.HANGHOAs on dsthich.MaMatHang equals hanghoa.MaMatHang
                          where (dsthich.MaKhachHang == makh) 
                          select hanghoa; 
            if (layhang.Count() == 0)
            {
                ViewBag.ThongBao = "Bạn vẫn chưa yêu thích sản phẩm nào.";
            }
            else
            {
                ViewBag.ThongBao = "Sản phẩm bạn đã thích.";
            }
            return View(layhang.ToPagedList(pagenum, pagesize));
        } 

        public ActionResult soLuongNguoiXem()
        {
            var soluong = db.NguoiXems.Sum(row => row.SoNguoiXem); 
            //ViewBag.songuoidangonline = Session["online"].ToString();
            ViewBag.songuoidangonline = Session["online"].ToString();
            ViewBag.soNguoiOnline = soluong;
            return PartialView();
        }
    } 
}