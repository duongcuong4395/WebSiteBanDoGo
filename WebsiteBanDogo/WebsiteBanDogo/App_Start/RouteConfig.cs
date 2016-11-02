using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebsiteBanDogo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}",
              new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
                name: "HoTro",
                url: "Ho-Tro-Khach-Hang",
                defaults: new { controller = "DogoStore", action = "HoTroKhachHang", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "GioiThieuCuaHang",
                url: "Gioi-Thieu-Cua-Hang",
                defaults: new { controller = "DogoStore", action = "GioiThieuCuaHang", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "LienHeCuaHang",
                url: "Lien-He-Cua-Hang",
                defaults: new { controller = "DogoStore", action = "LienHeCuaHang", id = UrlParameter.Optional }
            ); 

            routes.MapRoute(
                name: "XemChiTiet",
                url: "Xem-chi-Tiet",
                defaults: new { controller = "DoGo", action = "XemChiTiet", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SanPhamTheoLoaiDoGo",
                url: "Loai-Do-Go/San-Pham-Theo-Loai-Do-Go/{id}",
                defaults: new { controller = "LoaiDoGo", action = "SanPhamTheoLoaiDoGo", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "TimKiem",
                url: "Tim-Kiem-Theo-Ten/{id}",
                defaults: new { controller = "TimKiem", action = "KetQuaTimKiem", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "TimKiemTheoGia",
                url: "Tim-Kiem-Theo-Gia/{id}",
                defaults: new { controller = "TimKiem", action = "TimKiemTheoGiaMoi", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "GioHang",
                url: "Gio-Hang",
                defaults: new { controller = "GioHang", action = "GioHang", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DKDN",
                url: "Dang-Ky-Dang-Nhap",
                defaults: new { controller = "NguoiDung", action = "chucNangDKDN", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ThongTinTaiKhoan",
                url: "Khach-Hang/Thong-Tin-Khach-Hang",
                defaults: new { controller = "NguoiDung", action = "ThongTinTaiKhoan", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SuaThongTinKH",
                url: "Khach-Hang/Thong-Tin-Khach-Hang/Chinh-Sua-thong-Tin",
                defaults: new { controller = "NguoiDung", action = "SuaThongTinKH", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DoiMatKhau",
                url: "Khach-Hang/Doi-Mat-Khau",
                defaults: new { controller = "NguoiDung", action = "DoiMatKhau", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "LayMatKhau",
                url: "Quen-Mat-Khau",
                defaults: new { controller = "NguoiDung", action = "LayMatKhau", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DShangYeuThich",
                url: "Danh-Sach-San-Pham-Thich",
                defaults: new { controller = "DoGoStore", action = "DShangYeuThich", id = UrlParameter.Optional }
            );
 
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "DoGoStore", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
