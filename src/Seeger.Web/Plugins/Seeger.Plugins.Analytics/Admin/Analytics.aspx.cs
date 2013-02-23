using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Plugins.Analytics.Admin
{
    public partial class Analytics : BackendPageBase
    {
        public override bool VerifyAccess(Security.User user)
        {
            return user.HasPermission("Default", "DefaultModule", "EditAnalytics");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindView();
            }
        }

        private void BindView()
        {
            var settings = GlobalSettingManager.Instance;

            var analyticsCode = settings.GetValue(SettingKeys.AnalyticsCode);
            var enableAnalytics = settings.TryGetValue<bool>(SettingKeys.EnableAnalyticsCode, false);

            Code.Text = analyticsCode;
            EnableAnalyticsCode.Checked = enableAnalytics;
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            var settings = GlobalSettingManager.Instance;

            settings.SetValue(SettingKeys.AnalyticsCode, Code.Text);
            settings.SetValue(SettingKeys.EnableAnalyticsCode, EnableAnalyticsCode.Checked ? "true" : "false");

            settings.SubmitChanges();

            ShowMessage(Localize("Message.SaveSuccess"), MessageType.Success);
        }

        private void ShowMessage(string message, MessageType type)
        {
            var messageProvider = Page.Master as IMessageProvider;
            if (messageProvider != null)
            {
                messageProvider.ShowMessage(message, type);
            }
        }
    }
}