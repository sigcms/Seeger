using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Seeger.Licensing;
using Seeger.Globalization;
using System.Globalization;

namespace Seeger.Web.UI.Admin.Licensing
{
    public partial class CurrentDomainHint : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (LicensingService.CurrentLicense.IsValid && !LicensingService.CurrentLicense.IsDomainLicensed(Request.Url.Host))
            {
                Hint.Visible = true;
                Hint.Text = ResourceFolder.Global.GetValue("Licensing.CurrentDomainNotSupportedHint", CultureInfo.CurrentUICulture)
                                      .Replace("{ValidationPath}", "/Admin/Licensing/ValidateSystem.aspx")
                                      .Replace("{Domain}", Request.Url.Host);
            }
        }
    }
}