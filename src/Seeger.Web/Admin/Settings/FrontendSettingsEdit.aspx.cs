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

            PageExtension.Items.Add(new ListItem("(" + Localize("Common.NoExtension") + ")", String.Empty));
            PageExtension.DataSource = FrontendSettings.SupportedPageExtensions;
            PageExtension.DataBind();

            Multilingual.Checked = FrontendSettings.Multilingual;
            PageExtension.SelectedValue = FrontendSettings.PageExtension;

            CloseWebsite.Checked = FrontendSettings.IsWebsiteOffline;
            PageList.Rebind();
            PageList.SelectedPageId = FrontendSettings.OfflinePageId;

            if (!String.IsNullOrEmpty(FrontendSettings.DefaultLanguage))
            {
                FrontendLang.SelectedValue = FrontendSettings.DefaultLanguage;
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            FrontendSettings.Multilingual = Multilingual.Checked;
            FrontendSettings.PageExtension = PageExtension.SelectedValue;

            FrontendSettings.IsWebsiteOffline = CloseWebsite.Checked;
            if (CloseWebsite.Checked)
            {
                FrontendSettings.OfflinePageId = PageList.SelectedPageId;
            }

            FrontendSettings.DefaultLanguage = FrontendLang.SelectedValue;

            GlobalSettingManager.Instance.SubmitChanges();

            ((Management)Master).ShowMessage(Localize("Message.SaveSuccess"), MessageType.Success);
        }

        protected string GetOfflineUrlRowStyle()
        {
            return FrontendSettings.IsWebsiteOffline ? String.Empty : "display:none";
        }
    }
}