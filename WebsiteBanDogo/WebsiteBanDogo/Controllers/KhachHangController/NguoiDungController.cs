using BotDetect.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanDogo.Models; 
using System.Configuration;
using System.Security.Cryptography;
  

namespace WebsiteBanDogo.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung 
        QLDoGoCuongThaiDataContext db = new QLDoGoCuongThaiDataContext();

        [HttpGet]
        public ActionResult DangKyNguoiDung()
        {
            if (Session["Taikhoan"] != null)
            {
                ViewBag.ThongBao = "Bạn hãy đăng xuất để làm việc này.";
                return RedirectToAction("Index", "SachStore");
            }
            else
            {
                ViewBag.ThongBao = "Mời bạn chọn(click) vào ô Đăng ký."; 
                return RedirectToAction("chucNangDKDN", "NguoiDung");
            }
        }

        [HttpGet]
        public ActionResult chucNangDKDN()
        {
            if (Session["Taikhoan"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "DoGoStore"); 
            } 
        }

        //Post: Hàm DangKy(Post) nhận dữ liệu từ trang đăng ký và trhuwcj hiện việc tạo mới dữ liệu
        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaCode", "dangkyCapcha", "Mã xác nhận không đúng!")]
        public ActionResult DangKyNguoiDung(FormCollection formdangkyCollection, KHACHHANG kh)
        {  
            try
            {
                if(ModelState.IsValid)
                {
                    //gán các giá trị ngừoi dùng nhập liệu cho cấc biến.
                    var hoten = formdangkyCollection["HoTenKH"];
                    var tendangnhap = formdangkyCollection["TenDangNhap"];
                    var matkhau = formdangkyCollection["MatKhau"];
                    var nhaplaimatkhau = formdangkyCollection["NhapLaiMatKhau"];
                    var email = formdangkyCollection["Email"];
                    var dienthoai = formdangkyCollection["SoDienThoai"];
                    var sotaikhoan = formdangkyCollection["SoTaiKhoan"];
                    var diachi = formdangkyCollection["DiaChi"];
                    var ngaysinh = formdangkyCollection["NgaySinh"];


                    //Kiểm tra lỗi khi đăng ký
                    if (db.KHACHHANGs.Where(m => kh.TaiKhoan == tendangnhap).SingleOrDefault() != null)
                    {
                        ViewBag.ThongBaodk = "* Tên đăng nhập hoặc mật khẩu đã tồn tại.";
                        return RedirectToAction("chucNangDKDN", "NguoiDung");
                    } 

                    else if (db.KHACHHANGs.Where(m => m.Email == email).SingleOrDefault() != null)
                    {
                        ViewBag.ThongBaodk = "* Tên đăng nhập hoặc mật khẩu đã tồn tại.";
                        return RedirectToAction("chucNangDKDN", "NguoiDung");
                    }

                    else if (matkhau != nhaplaimatkhau)
                    {
                        ViewBag.ThongBaodk = ViewBag.ThongBao + "Nhập lại mật khẩu phải giống nhau.";
                        return RedirectToAction("chucNangDKDN", "NguoiDung");
                    } 
                    else
                    {
                        MvcCaptcha.ResetCaptcha("dangkyCapcha");
                        kh.TenKhachHang = hoten;
                        kh.TaiKhoan = tendangnhap;
                        kh.MatKhau = matkhau;
                        kh.SoDienThoai = dienthoai;
                        kh.Email = email;
                        kh.DiaChi = diachi;
                        kh.SoTaiKhoan = sotaikhoan;
                        kh.NgaySinh = DateTime.Parse(ngaysinh.ToString());
                        db.KHACHHANGs.InsertOnSubmit(kh);
                        db.SubmitChanges();
                        Session["Taikhoan"] = kh;
                        Session["TenKhachHang"] = kh.TenKhachHang;
                        Session["TenTK"] = kh.TaiKhoan;
                        Session["MaTKKH"] = kh.MaKhachHang;
                        return RedirectToAction("Index", "DoGoStore");
                    } 
                } 
                return RedirectToAction("chucNangDKDN", "NguoiDung");
            }
            catch (Exception error)
            {
                return RedirectToAction("chucNangDKDN", "NguoiDung");
            } 
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            if (Session["Taikhoan"] != null)
            {
                ViewBag.ThongBao = "Tài khoản của bạn đang được sử dụng ko thể thực hiện ĐĂNG NHẬP nữa.";
                return View();
            }
            else
            {
                ViewBag.ThongBao = "Bạn chưa đăng nhập tài khoản thì không thể đổi mật khẩu.";
                return RedirectToAction("chucNangDKDN", "NguoiDung");
            }  
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(FormCollection fc)
        {
            try { 
                if (ModelState.IsValid) {
                    var tendangnhap = fc["TenDangNhap"];
                    var matkhau = fc["MatKhau"];

                    //var MatKhauMaHoa = MaHoa(matkhau, true);
                    //Gans giá trị cho đối dtuwongj tạo mới      
                    KHACHHANG kh = db.KHACHHANGs.Where(n => n.TaiKhoan == tendangnhap && n.MatKhau == matkhau).FirstOrDefault();
                    if (tendangnhap == null)
                    {
                        ViewData["txtTenDangNhap"] = "Tên đăng nhập không bỏ trống.";
                    }
                    if (matkhau == null)
                    {
                        ViewData["txtMatKhau"] = "Mật khẩu không được bỏ trống.";
                    }
                    if (kh != null)
                    {
                        // ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                        Session["Taikhoan"] = kh;
                        Session["TenKhachHang"] = kh.TenKhachHang;
                        Session["TenTK"] = kh.TaiKhoan;
                        Session["MaTKKH"] = kh.MaKhachHang;
                        return RedirectToAction("Index", "DoGoStore");
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                    }
                } 
                return RedirectToAction("chucNangDKDN", "NguoiDung");
            }
            catch(Exception error) {
                return RedirectToAction("chucNangDKDN", "NguoiDung");
            }
        }

        public ActionResult ThongTinTaiKhoan()
        {
            if (Session["Taikhoan"] != null)
            { 
                KHACHHANG khachHang = db.KHACHHANGs.Where(n => n.TaiKhoan == Session["TenTK"].ToString()).FirstOrDefault();
                
                if (khachHang == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(khachHang);
            }
            else
            {
                return RedirectToAction("Index", "DoGoStore");
            }
        }

        [HttpGet]
        public ActionResult SuaThongTinKH()
        {
            KHACHHANG khachhang = db.KHACHHANGs.Where(n => n.TaiKhoan == Session["TenTK"].ToString()).FirstOrDefault();
             
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachhang);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SuaThongTinKH(FormCollection frmCollection, KHACHHANG khachhang)
        { 
            try
            { 
                if(ModelState.IsValid)
                {
                    var makhachhang = frmCollection["makhachHang"];
                    int id = int.Parse(makhachhang.ToString());
                    KHACHHANG kh = db.KHACHHANGs.Where(n => n.MaKhachHang == id).FirstOrDefault();

                    var tenkhachhang = frmCollection["TENKH"];

                    var email = frmCollection["EMAIL"];
                    var diachi = frmCollection["DIACHI"];
                    var sodienthoai = frmCollection["SODT"];
                    var sotaikhoan = frmCollection["SOTAIKHOAN"];
                    var ngaysinh = frmCollection["NGAYSINH"];

                    kh.MaKhachHang = id;
                    kh.TenKhachHang = tenkhachhang;
                    kh.Email = email;
                    kh.DiaChi = diachi;
                    kh.SoDienThoai = sodienthoai;
                    kh.SoTaiKhoan = sotaikhoan;
                    kh.NgaySinh = DateTime.Parse(ngaysinh);

                    if (ModelState.IsValid)
                    {
                        UpdateModel(kh);
                        db.SubmitChanges();
                        Session["Taikhoan"] = kh;
                        Session["TenKhachHang"] = kh.TenKhachHang;
                        Session["TenTK"] = kh.TaiKhoan;
                        Session["MaTKKH"] = kh.MaKhachHang;
                        return RedirectToAction("ThongTinTaiKhoan", "NguoiDung");
                    }
                } 
                return SuaThongTinKH(); 
            }
            catch (Exception error)
            {
                return RedirectToAction("ThongTinTaiKhoan", "NguoiDung");
            }
        }

        public ActionResult logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "DoGoStore");
        }

        [HttpGet]
        public ActionResult LayMatKhau()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaCode", "quenMatKhauCapcha", "Mã xác nhận không đúng!")]
        public ActionResult LayMatKhau(FormCollection frmColection)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    string mahoamatkhau = " ";
                    if (ModelState.IsValid)
                    {
                        var tendangnhap = frmColection["TenDangNhap"];
                        var Email = frmColection["Email"];
                        KHACHHANG kh = db.KHACHHANGs.Where(n => n.TaiKhoan == tendangnhap && n.Email == Email).FirstOrDefault();

                        if (kh != null)
                        {
                            MvcCaptcha.ResetCaptcha("quenMatKhauCapcha");
                            mahoamatkhau = MaHoaMD5(kh.MatKhau);
                            kh.MatKhau = mahoamatkhau;

                            ViewBag.ThongBao = "Để đảm bảo an toàn về tài khoản của bạn chúng tôi sẽ gửi mật khẩu mới tại đây.";
                            ViewBag.matkhaumoi = mahoamatkhau;
                            ViewBag.ThongBao2 = "Bạn nên lưu(copy/save) lại đoạn mã ở trên để đổi lại mật khẩu.";
                            ViewBag.ThongBao3 = "Chúc bạn có 1 ngày làm vui vẻ.";
                            UpdateModel(kh);
                            db.SubmitChanges();

                        }
                        else
                        {
                            ViewBag.ThongBao = "Tên đăng nhập khoặc email không đúng";
                        }
                    }
                } 
                return this.LayMatKhau();
            }
            catch (Exception error)
            {
                return this.LayMatKhau();
            } 
        } 
        //hash chuoi ra md5
        public string MaHoaMD5(string chuoi)
        {
            string str_md5 = "";
            byte[] mang = System.Text.Encoding.UTF8.GetBytes(chuoi);
            MD5CryptoServiceProvider my_md5 = new MD5CryptoServiceProvider();
            mang = my_md5.ComputeHash(mang);
            foreach (byte b in mang)
            {
                str_md5 += b.ToString("X2");
            }
            return str_md5;
        }

        [HttpGet]
        public ActionResult DoiMatKhau()
        {
            if (Session["Taikhoan"] != null)
            { 
                return View();
            }
            else
            {
                ViewBag.ThongBao = "Bạn chưa đăng nhập tài khoản thì không thể đổi mật khẩu."; 
                return RedirectToAction("DangNhap", "NguoiDung");
            } 
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DoiMatKhau(FormCollection formDoiMatKhauCollection)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var MatKhauCu = formDoiMatKhauCollection["MatKhauCu"];
                    var MatKhauMoi = formDoiMatKhauCollection["MatKhauMoi"];
                    var XacNhanMatKhau = formDoiMatKhauCollection["XacNhanMK"];
                    var makh = Session["MaTKKH"];
                    int mkh = int.Parse(makh.ToString());
                    string mkm = MatKhauMoi.ToString();

                    KHACHHANG kh = db.KHACHHANGs.Where(n => n.MaKhachHang == mkh && n.MatKhau == MatKhauCu).FirstOrDefault();
                    if (MatKhauMoi != XacNhanMatKhau)
                    {
                        ViewBag.ThongBao = "Nhập lại mật khẩu phải giống nhau.";
                    }
                    else
                    {
                        if (ModelState.IsValid)
                        {
                            kh.MatKhau = mkm;
                            UpdateModel(kh);
                            db.SubmitChanges();
                            ViewBag.ThongBao = "Bạn đã đổi mật khẩu thành công.";
                            return this.DoiMatKhau();
                        }
                    }
                } 
                return this.DoiMatKhau();
            }
            catch (Exception error)
            {
                ViewBag.ThongBao = "Mật khẩu Cũ không đúng.";
                return this.DoiMatKhau();
            } 
        } 
    }
}