using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Web.UI.Admin
{
    public class Heartbeat : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("OK");
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