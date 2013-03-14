using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using Seeger.Globalization;

namespace Seeger.Web.UI.Admin.Controls
{
    public partial class SeegerNews : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected string T(string key)
        {
            return ResourcesFolder.Global.GetValue(key, CultureInfo.CurrentCulture) ?? key;
        }
    }
}