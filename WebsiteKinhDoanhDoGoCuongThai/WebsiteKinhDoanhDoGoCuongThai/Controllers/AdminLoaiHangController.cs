using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebsiteKinhDoanhDoGoCuongThai.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebsiteKinhDoanhDoGoCuongThai.Controllers
{
    public class AdminLoaiHangController : Controller
    {
        // GET: AdminLoaiHang

        QLDoGoDataContext db = new QLDoGoDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoaiHangAdmin(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.LOAIHANGs.ToList().OrderBy(n => n.MaLoaiHang).ToPagedList(pageNumber, pageSize));
        }
    }
}