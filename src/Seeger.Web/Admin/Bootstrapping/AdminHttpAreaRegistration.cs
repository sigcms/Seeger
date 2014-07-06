using Seeger.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Seeger.Web.UI.Admin.Bootstrapping
{
    public class AdminHttpAreaRegistration : HttpAreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "admin";
            }
        }

        public override void RegisterArea(HttpAreaRegistrationContext context)
        {
            context.MapHttpRoute("AdminDefault", "api/admin/{controller}/{id}", new { id = RouteParameter.Optional });
        }
    }
}