using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.Events
{
    public class HttpRequestBegun : HttpApplicationEvent
    {
        public HttpRequestBegun(HttpApplication app)
            : base(app)
        {
        }
    }
}
