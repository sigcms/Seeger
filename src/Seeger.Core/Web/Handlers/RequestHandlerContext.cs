using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.Handlers
{
    public class RequestHandlerContext
    {
        public HttpContext HttpContext { get; private set; }

        public HttpRequest Request
        {
            get
            {
                return HttpContext.Request;
            }
        }

        public HttpResponse Response
        {
            get
            {
                return HttpContext.Response;
            }
        }

        public string TargetPath { get; set; }

        public RequestHandlerContext(HttpContext context)
        {
            HttpContext = context;
            TargetPath = context.Request.Path;
        }
    }
}
