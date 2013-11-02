using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.Events
{
    public class VaryByCustomStringRequested : HttpEvent
    {
        public string CustomParamName { get; private set; }

        public string Result { get; set; }

        public VaryByCustomStringRequested(string customParamName, HttpContextBase httpContext)
            : base(httpContext)
        {
            CustomParamName = customParamName;
        }
    }
}
