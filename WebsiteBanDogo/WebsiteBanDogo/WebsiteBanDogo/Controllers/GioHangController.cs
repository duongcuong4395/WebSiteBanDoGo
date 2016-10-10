using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanDogo.Models;

namespace WebsiteBanDogo.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang

        QLDoGoDataContext db = new QLDoGoDataContext();

        //Lấy giỏ hàng trong session["GioHang"]
        public List<GioHang> layGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>(); 
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }


        //Thêm hàng vào giỏ hàng tương ứng với mã mặt hàng
        public ActionResult ThemGioHang(string sMaMatHang, string strURL)
        {
            List<GioHang> lstGioHang = layGioHang();
            GioHang dsSanPham = lstGioHang.Find(n => n.sMaMatHang == sMaMatHang);
            if (dsSanPham == null)
            {
                dsSanPham = new GioHang(sMaMatHang);
                lstGioHang.Add(dsSanPham);
                return RedirectToAction("GioHang");
                //return Redirect(strURL);
            }
            else
            {
                dsSanPham.iSoLuong++;
                return Redirect(strURL);
            }
        }

        //Tính tổng số lượng hàng trong giỏ hàng
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }

        //Tính tổng tiền trong giỏ hàng
        private double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }

        //dang sách hàng trong giỏ hàng
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = layGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "DoGoStore");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView(lstGioHang);
        }

        //Xóa hàng trong giỏ hàng
        public ActionResult XoaGioHang(string MaMatHang=" ")
        {
            //Lấy giỏ hàng có trong session["GioHang"]
            List<GioHang> lstGioHang = layGioHang();
            //Kiểm tra hàng trong giỏ
            GioHang layHangHoa = lstGioHang.SingleOrDefault(n => n.sMaMatHang == MaMatHang);
            if (layHangHoa != null)
            {
                lstGioHang.RemoveAll(n => n.sMaMatHang == MaMatHang);
                return RedirectToAction("GioHang");
            }

            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "DoGoStore");
            }
            return RedirectToAction("GioHang");
        }

        //Cập nhật giỏ hàng
        public ActionResult CapNhatGioHang( FormCollection frmCollection, string MaMatHang=" " )
        {
            //Lấy giỏ hàng từ Session["GioHang"]
            List<GioHang> lstGioHang = layGioHang(); 
            //Kiểm tra sản phẩm đã có trong giỏ
            GioHang sp = lstGioHang.SingleOrDefault(n => n.sMaMatHang == MaMatHang);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(frmCollection["txtSoLuong"].ToString());  
            }
            return RedirectToAction("GioHang");
        }


        //Xóa tất cả sản phầm có trong giỏ hàng
        public ActionResult XoaTatCaGioHang()
        {
            //Lấy giỏ hàng từ session giỏ hàng
            List<GioHang> lstGioHang = layGioHang(); 
            lstGioHang.Clear(); 
            return RedirectToAction("Index", "DoGoStore");
        }

        [HttpGet]
        //Đặt hàng
        public ActionResult DatHang()
        {
            //Kiểm tra khách hàng đã đăng nhập chưa
            if (Session["Taikhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            //Kiểm tra hàng trong giỏ hàng
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "DoGoStore");
            }

            //Lấy giỏ hàng từ sesion["GioHang"]
            List<GioHang> lstGioHang = layGioHang();
            ViewBag.TongSoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }

       [HttpPost]
        public ActionResult DatHang(FormCollection frmCollection)
        {
            //Thêm đơn hàng
            var hoTenNguoiNhan = frmCollection["HoTenNguoiNgan"];
            var diaChiNguoiNhan = frmCollection["DiaChiNguoiNhan"];
            var sdtNguoiNhan = frmCollection["SDTNguoiNhan"];
            var ngayGiao = String.Format("{0:MM/dd/yyyy}", frmCollection["NgayGiao"]); 

            DONDATHANG dondathang = new DONDATHANG();
            KHACHHANG khachhang = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> lstGioHang = layGioHang();
             //thông tin khách hàng đặt hàng
            dondathang.MaKhachHang = khachhang.MaKhachHang;
            dondathang.NgayLap = DateTime.Now; 
            dondathang.NgayGiaoHang = DateTime.Parse(ngayGiao);
            dondathang.TinhTrangGiao = false;
            dondathang.TinhTrangThanhToan = false;
            dondathang.HinhThucThanhToan = false;
           //Người nhận
            dondathang.HoTenNguoiNhan = hoTenNguoiNhan;
            dondathang.DiaChiNguoiNhan = diaChiNguoiNhan;
            dondathang.SoDienThoaiNguoiNhan = sdtNguoiNhan; 
            db.DONDATHANGs.InsertOnSubmit(dondathang);
            db.SubmitChanges(); 
            //Chi chi tiết đơn hàng
            foreach (var hang in lstGioHang)
            {
                CHITIETHANGHOA chitietdonhang = new CHITIETHANGHOA();
                chitietdonhang.MaDonDatHang = dondathang.MaDonDatHang;
                chitietdonhang.MaMatHang = hang.sMaMatHang;
                chitietdonhang.SoLuong = hang.iSoLuong;
                chitietdonhang.SoTien = (decimal)hang.dDonGia;
                db.CHITIETHANGHOAs.InsertOnSubmit(chitietdonhang);
            }
            db.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang"); 
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}