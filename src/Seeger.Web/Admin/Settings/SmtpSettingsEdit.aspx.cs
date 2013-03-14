using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Settings
{
    public partial class SmtpSettingsEdit : AdminPageBase
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "SiteSetting", "EmailSetting");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var setting = GlobalSettingManager.Instance.Smtp;

                SmtpServer.Text = setting.Server;
                Port.Text = setting.Port.ToString();
                SenderName.Text = setting.SenderName;
                SenderEmail.Text = setting.SenderEmail;
                AccountName.Text = setting.AccountName;
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var setting = GlobalSettingManager.Instance.Smtp;

            setting.Server = SmtpServer.Text.Trim();
            setting.Port = Convert.ToInt32(Port.Text);
            setting.SenderName = SenderName.Text.Trim();
            setting.SenderEmail = SenderEmail.Text.Trim();
            setting.AccountName = AccountName.Text.Trim();

            if (AccountPassword.Text.Length > 0)
            {
                setting.Password = AccountPassword.Text;
            }

            GlobalSettingManager.Instance.SubmitChanges();

            ((IMessageProvider)Master).ShowMessage(T("Message.SaveSuccess"), MessageType.Success);
        }
    }
}