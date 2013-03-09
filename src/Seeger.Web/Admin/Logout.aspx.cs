using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.Admin
{
    public partial class Logout : Seeger.Web.UI.AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticationService.SignOut();
            AuthenticationService.RedirectToLoginPage();
        }
    }
}