﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace Seeger.Web.Events
{
    public class HttpSessionStarted : HttpApplicationEvent
    {
        public HttpSessionState Session
        {
            get
            {
                return Application.Session;
            }
        }

        public HttpSessionStarted(HttpApplication application)
            : base(application)
        {
        }
    }
}
