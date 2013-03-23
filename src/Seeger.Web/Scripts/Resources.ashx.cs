using Newtonsoft.Json;
using Seeger.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Web.UI.Scripts
{
    public class Resources : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/javascript";

            var map = ResourceFolder.Global.GetResourceDictionary(AdminSession.Current.UICulture);
            var script = String.Format("sig.Resources.init({0})", JsonConvert.SerializeObject(map));
            context.Response.Write(script);
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