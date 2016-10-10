using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteBanDogo
{
    /// <summary>
    /// Summary description for KeepAlive_ashx
    /// </summary>
    public class KeepAlive_ashx : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}