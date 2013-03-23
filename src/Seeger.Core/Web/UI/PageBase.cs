using NHibernate;
using Seeger.Config;
using Seeger.Data;

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
    }

}
