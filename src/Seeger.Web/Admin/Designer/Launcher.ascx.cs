using Seeger.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.Admin.Designer
{
    public partial class Launcher : UserControl
    {
        protected string DesignerUrl
        {
            get
            {
                var url = Request.RawUrl;
                if (url.IndexOf('?') < 0)
                {
                    url += "?";
                }
                else
                {
                    url += "&";
                }

                return url += "design=true";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected string T(string key)
        {
            return ResourceFolder.Global.GetValue(key, AdminSession.Current.UICulture ?? CultureInfo.CurrentUICulture) ?? key;
        }
    }
}