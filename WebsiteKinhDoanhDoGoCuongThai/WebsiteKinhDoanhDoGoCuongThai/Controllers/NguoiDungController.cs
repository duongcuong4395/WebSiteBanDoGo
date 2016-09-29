using BotDetect.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteKinhDoanhDoGoCuongThai.Models;
using Facebook;
using System.Configuration; 

//2 thư viện dùng để mả hóa và giải mã.
using System.Security.Cryptography;
using System.Text;
using System.Web.Security; 

namespace WebsiteKinhDoanhDoGoCuongThai.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung 
        QLDoGoDataContext db = new QLDoGoDataContext();  

        [HttpGet]
        public ActionResult DangKyNguoiDung()
        {
            return View();
        }


        //Hàm Mã hóa 1 chuỗi String
        public string MaHoa(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes("hoidapit.com.vn"));
            }
            else keyArray = Encoding.UTF8.GetBytes("hoidapit.com.vn");
            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        //Post: Hàm DangKy(Post) nhận dữ liệu từ trang đăng ký và trhuwcj hiện việc tạo mới dữ liệu
        [HttpPost]
        [AllowAnonymous]
        [CaptchaValidation("CaptchaCode", "dangkyCapcha", "Mã xác nhận không đúng!")] 
        public ActionResult DangKyNguoiDung(FormCollection formdangkyCollection, KHACHHANG kh)
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

            //Mã hóa mật khẩu nhập vào trước khi thêm vào cơ sở dữ liệu.
            string matKhauMaHoa = MaHoa(matkhau, true);
              
            //Kiểm tra lỗi khi đăng ký
            if (db.KHACHHANGs.Where(m => kh.TaiKhoan == tendangnhap || m.Email==email).FirstOrDefault() != null)
            {
                ViewBag.ThongBao = "* Tên đăng nhập hoặc mật khẩu đã tồn tại.";
            }
                 
            //kiểm tra mat khau vs mat khau nhap lai co trung nhau khong 
            else if (matkhau != nhaplaimatkhau)
            {
                ViewBag.ThongBao = ViewBag.ThongBao + "Nhập lại mật khẩu phải giống nhau.";
            }  

            else
            {
                //Gán các giá trị vừa tạo mới vào khach hàng
                kh.TenKhachHang = hoten;
                kh.TaiKhoan = tendangnhap;
                kh.MatKhau = matKhauMaHoa;
                kh.SoDienThoai = dienthoai;
                kh.Email = email;
                kh.DiaChi = diachi;
                kh.SoTaiKhoan = sotaikhoan;
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKyNguoiDung();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection formDangNhapCollection)
        { 
            var tendangnhap = formDangNhapCollection["TenDangNhap"];
            var matkhau = formDangNhapCollection["MatKhau"];

            var MatKhauMaHoa = MaHoa(matkhau, true);
            //Gans giá trị cho đối dtuwongj tạo mới      
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == tendangnhap && n.MatKhau == MatKhauMaHoa); 
            if (tendangnhap == null)
            {
                ViewData["txtTenDangNhap"] = "Tên đăng nhập không bỏ trống.";
            } 
            if (matkhau == null) {
                ViewData["txtMatKhau"] = "Mật khẩu không được bỏ trống.";
            } 
            if (kh != null)
            {
                // ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                Session["Taikhoan"] = kh;
                Session["TenKhachHang"] = kh.TenKhachHang;
                Session["TenTK"] = kh.TaiKhoan;
                Session["MaTKKH"] = kh.MaKhachHang;
                //return RedirectToAction("Index", "DoGoStore");
                return RedirectToAction("Index", "DoGoStore");
            }
            else
            {
                ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }            
            return View();
        }
 
         public ActionResult ThongTinTaiKhoan()
        {
            if (Session["Taikhoan"] != null)
            {
                //lấy đối tượng sản phẩm ra theo mã
                KHACHHANG khachHang = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == Session["TenTK"].ToString());
                //ViewBag.MaKhachHang = khachHang.MaKhachHang;
                if (khachHang == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(khachHang);
            }
            else
            {
                return RedirectToAction("DangNhap", "Nguoidung"); 
            }    
        }

        [HttpGet]
         public ActionResult SuaThongTinKH()
         {
             KHACHHANG khachhang = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == Session["TenTK"].ToString());
             //string referer = Request.ServerVariables["HTTP_REFERER"];
             //if (string.IsNullOrEmpty(referer))
             //{
             //    return RedirectToAction("Phanquyen", "LuckyStore");
             //}
             if (khachhang == null)
             {
                 Response.StatusCode = 404;
                 return null;
             }
             return View(khachhang);
         }

        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult SuaThongTinKH(FormCollection frmCollection, KHACHHANG khachhang)
        {
            KHACHHANG kh = db.KHACHHANGs.Where(n => n.MaKhachHang == khachhang.MaKhachHang).FirstOrDefault();
            if (ModelState.IsValid)
            {
                UpdateModel(kh);
                db.SubmitChanges();
            }
            return RedirectToAction("ThongTinTaiKhoan", "NguoiDung");
        } 

        public ActionResult logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "DoGoStore");
        }

        public ActionResult LayLaiMatKhau()
        {
            return View();
        } 
    }
}