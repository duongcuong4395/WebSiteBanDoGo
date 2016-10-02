using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteKinhDoanhDoGoCuongThai.Models;

using PagedList;
using PagedList.Mvc;

namespace WebsiteKinhDoanhDoGoCuongThai.Controllers
{
    [RequireHttps]
    public class DoGoStoreController : Controller
    {
        // GET: DoGoStore

        QLDoGoDataContext db = new QLDoGoDataContext();

        private List<HANGHOA> layHangHoa( )
        {
            return db.HANGHOAs.OrderByDescending(a => a.MaMatHang).ToList();
        }

        public ActionResult Index(int? page)
        {
            //Tạo biến Quy định sô sản phẩm trên 1 trang
            int pagesize = 9; 
            //Tạo biến số sang
            int pagenum = (page ?? 1 );

            // Lay ra 6 san pham
            var sanpham = layHangHoa();
            return View(sanpham.ToPagedList(pagenum, pagesize));
        }

        public ActionResult hienSanPham()
        {
            return View( );
        }

        [HttpGet]
        public ActionResult LienHeCuaHang(){
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

        public ActionResult layHangBanChay()
        {
            return PartialView();
        } 
        public ActionResult layHangBanChayitem()
        {
            return PartialView(db.HANGHOAs.Where(n => n.BanChay == true).OrderByDescending(a => a.MaMatHang).Take(5).ToList());
        }
    }
}