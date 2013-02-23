using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Seeger.Data;
using Seeger.Caching;
using NHibernate;

namespace Seeger.Web.UI
{
    public class UserControlBase : System.Web.UI.UserControl
    {
        public FrontendSettings FrontendSettings
        {
            get { return GlobalSettingManager.Instance.FrontendSettings; }
        }

        protected ISession NhSession
        {
            get
            {
                return NhSessionManager.GetCurrentSession();
            }
        }
    }

}
