using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Seeger.Globalization;
using Seeger.Licensing;
using Seeger.Security;
using Seeger.Caching;

namespace Seeger.Web.UI.Admin.Settings
{
    public partial class FrontendSettingsEdit : AdminPageBase
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "SiteSetting", "FrontendSetting");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindForm();
            }
        }

        private void BindForm()
        {
            FrontendLang.Items.Add(new ListItem("(" + Localize("Common.AutoDetect") + ")", String.Empty));
            FrontendLang.DataSource = FrontendLanguageCache.From(NhSession).Languages;
            FrontendLang.DataBind();

            Multilingual.Checked = FrontendSettings.Multilingual;

            if (!String.IsNullOrEmpty(FrontendSettings.DefaultLanguage))
            {
                FrontendLang.SelectedValue = FrontendSettings.DefaultLanguage;
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            FrontendSettings.Multilingual = Multilingual.Checked;
            FrontendSettings.DefaultLanguage = FrontendLang.SelectedValue;

            GlobalSettingManager.Instance.SubmitChanges();

            ((Management)Master).ShowMessage(Localize("Message.SaveSuccess"), MessageType.Success);
        }
    }
}