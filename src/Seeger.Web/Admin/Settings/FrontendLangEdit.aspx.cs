using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using Seeger.Web.UI.DataManagement;
using Seeger.Data;
using Seeger.Globalization;
using Seeger.Licensing;
using Seeger.Caching;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Settings
{
    public partial class FrontendLangEdit : DetailPageBase<FrontendLanguage>
    {
        public override bool VerifyAccess(User user)
        {
            return LicensingService.CurrentLicense.CmsEdition.IsFeatureAvailable(Features.Multilingual)
                && user.HasPermission(null, "SiteSetting", "FrontendLanguage");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override object CreateKey(string keyStringValue)
        {
            return keyStringValue;
        }

        protected override void OnSubmitted()
        {
            Response.Redirect("FrontendLangList.aspx", true);
        }

        protected override void BindSubmitEventHandler(EventHandler handler)
        {
            SubmitButton.Click += handler;
        }

        public override void InitView(FrontendLanguage entity)
        {
            LanguageList.DataSource = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            LanguageList.DataBind();

            if (FormState == FormState.EditItem)
            {
                LanguageList.SelectedValue = entity.Name;
                LanguageList.Enabled = false;
            }

            DisplayName.Text = entity.DisplayName;
            Domain.Text = entity.BindedDomain;
        }

        public override void UpdateObject(FrontendLanguage entity)
        {
            if (FormState == FormState.AddItem)
            {
                entity.Name = LanguageList.SelectedValue;
            }
            entity.DisplayName = DisplayName.Text.Trim();
            entity.BindedDomain = Domain.Text;
        }

        protected void LanguageDuplicateValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (FormState == FormState.AddItem)
            {
                e.IsValid = !FrontendLanguageCache.From(NhSession).Contains(e.Value);
            }
            else
            {
                e.IsValid = true;
            }
        }
    }
}