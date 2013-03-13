using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Data;
using Seeger.Security;
using Seeger.Web.UI.Grid;

namespace Seeger.Web.UI.Admin.Urls
{
    public partial class CustomRedirectList : AjaxGridPageBase
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "CustomRedirect", "View");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}