using Seeger.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.Events
{
    public abstract class HttpApplicationEvent : IEvent
    {
        public HttpApplication Application { get; private set; }

        public HttpContext Context
        {
            get
            {
                return Application.Context;
            }
        }

        public HttpRequest Request
        {
            get
            {
                return Application.Request;
            }
        }

        public HttpResponse Response
        {
            get
            {
                return Application.Response;
            }
        }

        protected HttpApplicationEvent(HttpApplication application)
        {
            Require.NotNull(application, "application");
            Application = application;
        }
    }
}
