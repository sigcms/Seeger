using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using Seeger.Data;
using Seeger.Globalization;

namespace Seeger.Web.UI
{
    public partial class Login : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = Localize("Login.PageTitle");
        }

        protected string Localize(string key)
        {
            return ResourcesFolder.Global.GetValue(key, CultureInfo.CurrentUICulture);
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            var user = AuthenticationService.Login(Name.Text.Trim(), Password.Text, false);

            if (user == null)
            {
                Message.Visible = true;
                Message.Text = Localize("Login.LoginFailed");
            }
            else
            {
                Response.Redirect("~/Admin/Default.aspx");
            }
        }
    }
}