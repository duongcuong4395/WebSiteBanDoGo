using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanDogo.Models;

using PagedList;
using PagedList.Mvc;

namespace WebsiteBanDogo.Controllers
{
    public class AdminGioiThieuLienHeController : Controller
    {
        // GET: AdminGioiThieuLienHe

        QLDoGoCuongThaiDataContext data = new QLDoGoCuongThaiDataContext();
        public ActionResult Index()
        {
            return View();
        }

        private List<GIOITHIEU> layGT()
        {
            return data.GIOITHIEUs.ToList();
        }

        public ActionResult GioiThieu()
        {
            var gt = layGT();
            return PartialView(gt);
        }

        [HttpGet]
        public ActionResult SuaGioiThieu(int MaGioiThieu)
        {
            if (Session["TKAdmin"] != null)
            {
                if (bool.Parse(Session["PhanQuyenAdmin"].ToString()) == true)
                {
                    GIOITHIEU gt = data.GIOITHIEUs.Where(n => n.MaGioiThieu == MaGioiThieu).FirstOrDefault();
                    ViewBag.MaGioiThieu = gt.MaGioiThieu;
                    if (gt == null)
                    {
                        Response.StatusCode = 404;
                        return null;
                    }
                    return View(gt);
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
        public ActionResult SuaGioiThieu(GIOITHIEU lh, FormCollection frm)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if (Session["TKAdmin"] != null)
                    {
                        GIOITHIEU gt = data.GIOITHIEUs.Where(n => n.MaGioiThieu == lh.MaGioiThieu).FirstOrDefault();
                        var tieude = frm["TieuDe"];
                        var noidung = frm["NoiDung"];
                        var hotline = frm["Hotline"];
                        if (ModelState.IsValid)
                        {
                            gt.TieuDe = tieude;
                            gt.NoiDung = noidung;
                            gt.Hotline = hotline;
                            UpdateModel(gt);
                            data.SubmitChanges();
                            return RedirectToAction("Index", "AdminQL");
                        }
                        return View(gt);
                    }
                    else
                    {
                        return RedirectToAction("DangNhap", "AdminQL");
                    }
                } 
                return RedirectToAction("DangNhap", "AdminQL");
            }
            catch (Exception error)
            {
                return RedirectToAction("DangNhap", "AdminQL");
            }
            
        }

        private List<HOMTHU> layHT()
        {
            return data.HOMTHUs.ToList();
        }

        public ActionResult HomThu()
        {
            var ht = layHT();
            var soluongthu = data.HOMTHUs.Count();
            ViewBag.soluongthu = soluongthu;
            return PartialView(ht);
        }
    }
}