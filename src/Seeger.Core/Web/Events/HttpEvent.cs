using Seeger.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.Events
{
    public abstract class HttpEvent : IEvent
    {
        public HttpContextBase HttpContext { get; private set; }

        public HttpRequestBase Request
        {
            get
            {
                return HttpContext.Request;
            }
        }

        public HttpResponseBase Response
        {
            get
            {
                return HttpContext.Response;
            }
        }

        protected HttpEvent(HttpContextBase httpContext)
        {
            Require.NotNull(httpContext, "httpContext");
            HttpContext = httpContext;
        }
    }
}
