using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanDogo.Models; 
 
namespace WebsiteBanDogo.Controllers
{
    public class DoGoController : Controller
    {
        QLDoGoDataContext db = new QLDoGoDataContext();
        public ViewResult XemChiTiet(string id)
        {
            HANGHOA hang = db.HANGHOAs.SingleOrDefault(n => n.MaMatHang == id);
            if (hang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(hang);
        } 
    }
}