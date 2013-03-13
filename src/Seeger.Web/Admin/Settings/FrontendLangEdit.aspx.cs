using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Seeger.Data;
using Seeger.Globalization;
using Seeger.Licensing;
using Seeger.Caching;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Settings
{
    public partial class FrontendLangEdit : AdminPageBase
    {
        protected string LangName
        {
            get
            {
                return Request.QueryString["name"];
            }
        }

        public override bool VerifyAccess(User user)
        {
            return LicensingService.CurrentLicense.CmsEdition.IsFeatureAvailable(Features.Multilingual)
                && user.HasPermission(null, "SiteSetting", "FrontendLanguage");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(LangName))
                {
                    InitView(NhSession.Get<FrontendLanguage>(LangName));
                }
                else
                {
                    InitView(new FrontendLanguage());
                }
            }
        }

        public void InitView(FrontendLanguage entity)
        {
            LanguageList.DataSource = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            LanguageList.DataBind();

            if (!String.IsNullOrEmpty(LangName))
            {
                LanguageList.SelectedValue = entity.Name;
                LanguageList.Enabled = false;
            }

            DisplayName.Text = entity.DisplayName;
            Domain.Text = entity.BindedDomain;
        }

        public void UpdateObject(FrontendLanguage entity)
        {
            if (String.IsNullOrEmpty(LangName))
            {
                entity.Name = LanguageList.SelectedValue;
            }
            entity.DisplayName = DisplayName.Text.Trim();
            entity.BindedDomain = Domain.Text;
        }

        protected void LanguageDuplicateValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (String.IsNullOrEmpty(LangName))
            {
                e.IsValid = !FrontendLanguageCache.From(NhSession).Contains(e.Value);
            }
            else
            {
                e.IsValid = true;
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(LangName))
            {
                var lang = NhSession.Get<FrontendLanguage>(LangName);
                UpdateObject(lang);
            }
            else
            {
                var lang = new FrontendLanguage();
                UpdateObject(lang);
                NhSession.Save(lang);
            }

            NhSession.Commit();

            Response.Redirect("FrontendLangList.aspx");
        }
    }
}