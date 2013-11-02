using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.Events
{
    public class ApplicationErrorOccurred : HttpApplicationEvent
    {
        public Exception LastError
        {
            get
            {
                return Application.Server.GetLastError();
            }
        }

        public ApplicationErrorOccurred(HttpApplication application)
            : base(application)
        {
        }
    }
}
