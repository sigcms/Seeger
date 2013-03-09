using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Licensing;
using Seeger.Globalization;
using System.Globalization;
using Seeger.Caching;
using Seeger.Data;

namespace Seeger.Web.UI.Admin.Licensing
{
    public partial class DomainBindingHint : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (GlobalSettingManager.Instance.FrontendSettings.Multilingual)
            {
                var languageCache = FrontendLanguageCache.From(Database.GetCurrentSession());

                if (languageCache.Languages.Any(it => !String.IsNullOrEmpty(it.BindedDomain) 
                    && LicensingService.CurrentLicense.IsDomainLicensed(it.BindedDomain) == false))
                {
                    Hint.Visible = true;
                    Hint.Text = ResourcesFolder.Global.GetValue("Licensing.DomainBindingNotSupportedHint", CultureInfo.CurrentUICulture)
                                          .Replace("{ValidationPath}", CmsVirtualPath.GetFull("/Admin/Licensing/ValidateSystem.aspx"))
                                          .Replace("{FrontendLangListPath}", CmsVirtualPath.GetFull("/Admin/Settings/FrontendLangList.aspx"));
                }
            }
        }
    }
}