using Seeger.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Comments.Api
{
    public class ApiAreaRegistration : HttpAreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "cmt";
            }
        }

        public override void RegisterArea(HttpAreaRegistrationContext context)
        {
            context.MapHttpRoute(
                name: AreaName + "_default",
                routeTemplate: "api/cmt/{controller}/{id}",
                defaults: new { id = System.Web.Http.RouteParameter.Optional }
                );
        }
    }
}