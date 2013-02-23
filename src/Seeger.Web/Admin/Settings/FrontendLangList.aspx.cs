using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Seeger.Web.UI.DataManagement;
using Seeger.Data;
using Seeger.Licensing;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Settings
{
    public partial class FrontendLangList : ListPageBase<FrontendLanguage>
    {
        public override bool VerifyAccess(User user)
        {
            return LicensingService.CurrentLicense.CmsEdition.IsFeatureAvailable(Features.Multilingual)
                && user.HasPermission(null, "SiteSetting", "FrontendLanguage");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override string EditingPageUrl
        {
            get
            {
                return "FrontendLangEdit.aspx";
            }
        }

        protected override GridView GridView
        {
            get
            {
                return Grid;
            }
        }

        protected string GetBindedDomainCellHtml(object dataItem)
        {
            FrontendLanguage language = (FrontendLanguage)dataItem;

            string html = language.BindedDomain;

            if (!String.IsNullOrEmpty(language.BindedDomain) && !LicensingService.CurrentLicense.IsDomainLicensed(language.BindedDomain))
            {
                html += "<span class='text-warning'> [" + Localize("Licensing.DomainNotIncludedInLicense") + "]</span>";
            }

            return html;
        }
    }
}