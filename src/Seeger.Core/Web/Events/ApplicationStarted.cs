using Seeger.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.Events
{
    public class ApplicationStarted : HttpApplicationEvent
    {
        public ApplicationStarted(HttpApplication application)
            : base(application)
        {
        }
    }
}
