using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using Seeger.Globalization;

namespace Seeger.Web.UI
{
    public partial class Offline : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = Localize("Offline.PageTitle", "Maintaining...");
        }

        protected string OfflineHint
        {
            get
            {
                return Localize("Offline.OfflineHint", "Maintaining...");
            }
        }

        protected string Localize(string key, string defaultValue)
        {
            return ResourceFolder.Global.GetValue(key, CultureInfo.CurrentUICulture) ?? defaultValue;
        }
    }
}