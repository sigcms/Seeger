using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Seeger.Data;
using Seeger.Caching;
using NHibernate;

namespace Seeger.Web.UI
{
    public class PageBase : System.Web.UI.Page
    {
        protected ISession NhSession
        {
            get
            {
                return Database.GetCurrentSession();
            }
        }

        protected FrontendSettings FrontendSettings
        {
            get { return GlobalSettingManager.Instance.FrontendSettings; }
        }

        private ScriptsHelper _scripts;

        public ScriptsHelper Scripts
        {
            get
            {
                if (_scripts == null)
                {
                    _scripts = new ScriptsHelper(Context);
                }
                return _scripts;
            }
        }
    }

}
