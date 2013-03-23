using NHibernate;
using Seeger.Config;
using Seeger.Data;
using System.Globalization;

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
                return Database.GetCurrentSession();
            }
        }

        protected virtual string T(string key)
        {
            return T(key, CultureInfo.CurrentUICulture);
        }

        protected virtual string T(string key, CultureInfo culture)
        {
            return SmartLocalizer.GetForCurrentRequest().Localize(key, culture);
        }
    }

}
