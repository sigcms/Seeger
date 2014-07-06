using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Seeger.Web.Http;
using System.Web.Http.Dispatcher;
using Seeger.Web.Http.Dispatcher;

namespace Seeger.Web
{
    public static class WebApiConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IHttpControllerSelector), new AreaHttpControllerSelector(config));

            HttpAreaRegistration.RegisterAllAreas();
        }
    }
}