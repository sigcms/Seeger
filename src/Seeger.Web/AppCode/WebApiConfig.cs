using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Seeger.Web.Http;
using System.Web.Routing;
using System.Web.Http.Dispatcher;
using Seeger.Plugins;
using System.Web.Compilation;
using System.Reflection;

namespace Seeger.Web
{
    public class SeegerAssembliesResolver : IAssembliesResolver
    {
        public ICollection<System.Reflection.Assembly> GetAssemblies()
        {
            return BuildManager.GetReferencedAssemblies().OfType<Assembly>().ToList();
        }
    }

    public static class WebApiConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IAssembliesResolver), new SeegerAssembliesResolver());

            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            config.Filters.Add(new HandleErrorAttribute());

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "WebApi_Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}