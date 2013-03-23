using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using Seeger.Globalization;

namespace Seeger.Web.UI.Admin.My
{
    public partial class Profile : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            foreach (var theme in AdminSkins.Skins)
            {
                SkinList.Items.Add(new ListItem(theme.DisplayName.Localize(), theme.Name));
            }

            LanguageList.DataSource = ResourceFolder.Global.Cultures;
            LanguageList.DataBind();

            UserName.Text = CurrentUser.UserName;
            Nick.Text = CurrentUser.Nick;
            Email.Text = CurrentUser.Email;
            
            if (CurrentUser.Skin != null)
            {
                SkinList.SelectedValue = CurrentUser.Skin.Name;
            }
            if (CurrentUser.Language != null)
            {
                LanguageList.SelectedValue = CurrentUser.Language;
            }
            else
            {
                LanguageList.SelectedValue = CultureInfo.CurrentUICulture.Name;
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            CurrentUser.Nick = Nick.Text.Trim();
            CurrentUser.Email = Email.Text;
            CurrentUser.Skin = AdminSkins.Find(SkinList.SelectedValue);
            CurrentUser.Language = LanguageList.SelectedValue;

            if (Password.Text.Length > 0)
            {
                CurrentUser.UpdatePassword(Password.Text);
            }

            NhSession.Commit();

            ((IMessageProvider)Master).ShowMessage(T("Message.SaveSuccess"), MessageType.Success);
        }
    }
}