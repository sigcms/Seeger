using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Seeger.Security;
using Seeger.Globalization;

namespace Seeger.Web.UI
{
    public class BackendUserControlBase : UserControlBase
    {
        public User CurrentUser
        {
            get { return AdministrationSession.User; }
        }

        public AdministrationSession AdministrationSession
        {
            get { return AdministrationSession.Current; }
        }        
        
        protected string Localize(string key)
        {
            return Localize(key, CultureInfo.CurrentUICulture);
        }

        protected string Localize(string key, CultureInfo culture)
        {
            return ResourcesFolder.Global.GetValue(key, culture) ?? key;
        }
    }
}
