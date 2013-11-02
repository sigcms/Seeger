using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.Events
{
    public class HttpRequestEnded : HttpApplicationEvent
    {
        public HttpRequestEnded(HttpApplication application)
            : base(application)
        {
        }
    }
}
