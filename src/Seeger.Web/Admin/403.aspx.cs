using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Security;

namespace Seeger.Web.UI.Admin
{
    public partial class _403 : BackendPageBase
    {
        public override bool VerifyAccess(User user)
        {
            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}