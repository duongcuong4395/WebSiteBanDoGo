using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteBanDogo.Models;

namespace WebsiteBanDogo.Models
{
    public class GioHang
    {
        QLDoGoDataContext db = new QLDoGoDataContext();

        public string sMaMatHang { set; get; } 

        public string sTenHangHoa {set; get; } 

        public string sAnhBia {set; get; } 

        public Double dDonGia { set; get; }

        public Double dKHuyenMai { set; get; }

        public Double dGiaMoi { set; get; }

        public int iSoLuong{ set; get; }

        public Double dThanhTien
        {
            get { if (dGiaMoi == 0 || dGiaMoi<dDonGia)
                    {
                        return iSoLuong * dGiaMoi;
                        
                    }
                else {
                        return iSoLuong * dDonGia;
                    }
                 }
        }

        public GioHang(string MaMatHang)
        {
            sMaMatHang = MaMatHang;
            HANGHOA hang = db.HANGHOAs.Single(n => n.MaMatHang == sMaMatHang);
            sTenHangHoa = hang.TenMatHang;
            sAnhBia = hang.HinhAnh;
            dDonGia =  double.Parse(hang.DonGia.ToString()); 
            dGiaMoi = double.Parse(hang.GiaMoi.ToString()); 
            iSoLuong = 1;
        }
    }
}