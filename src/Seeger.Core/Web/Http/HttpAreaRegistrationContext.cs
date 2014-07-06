using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Seeger.Web.Http
{
    public class HttpAreaRegistrationContext
    {
        public string AreaName { get; private set; }

        public HttpRouteCollection Routes { get; private set; }

        public HttpAreaRegistrationContext(string areaName, HttpRouteCollection routes)
        {
            AreaName = areaName;
            Routes = routes;
        }

        public IHttpRoute MapHttpRoute(string name, string routeTemplate)
        {
            return MapHttpRoute(name, routeTemplate, null, null);
        }

        public IHttpRoute MapHttpRoute(string name, string routeTemplate, object defaults)
        {
            return MapHttpRoute(name, routeTemplate, defaults, null);
        }

        public IHttpRoute MapHttpRoute(string name, string routeTemplate, object defaults, object constraints)
        {
            var dataTokens = new Dictionary<string, object>
            {
                { "area", AreaName }
            };

            var httpRoute = Routes.CreateRoute(routeTemplate, new HttpRouteValueDictionary(defaults), new HttpRouteValueDictionary(constraints), dataTokens, null);
            Routes.Add(name, httpRoute);

            return httpRoute;
        }
    }
}
