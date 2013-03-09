using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Seeger.Security;
using Seeger.Globalization;

namespace Seeger.Web.UI
{
    public class AdminUserControlBase : UserControlBase
    {
        public User CurrentUser
        {
            get { return AdminSession.User; }
        }

        public AdminSession AdminSession
        {
            get { return AdminSession.Current; }
        }
    }
}
