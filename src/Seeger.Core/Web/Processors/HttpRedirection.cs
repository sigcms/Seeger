using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.Processors
{
    public class HttpRedirection
    {
        public string RedirectUrl { get; set; }

        public RedirectMode RedirectMode { get; set; }
    }
}
