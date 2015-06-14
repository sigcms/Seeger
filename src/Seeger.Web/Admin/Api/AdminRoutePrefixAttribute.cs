using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Seeger.Web.UI.Admin.Api
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AdminRoutePrefixAttribute : RoutePrefixAttribute
    {
        public AdminRoutePrefixAttribute(string prefix)
            : base("api/admin/" + prefix)
        {
        }
    }
}