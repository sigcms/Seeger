using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using Seeger.Licensing;
using Seeger.Globalization;

namespace Seeger.Web.UI.Admin.Licensing
{
    public partial class InvalidLicenseHint : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!LicensingService.CurrentLicense.IsValid)
                {
                    Hint.Visible = true;
                    Hint.Text = ResourcesFolder.Global
                                          .GetValue("Licensing.InvalidLicenseHint", CultureInfo.CurrentCulture)
                                          .Replace("{ValidationPath}", "/Admin/Licensing/ValidateSystem.aspx")
                                          .Replace("{PurchaseUrl}", SeegerUrls.Purchase);
                }
            }
        }
    }
}