using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Seeger.Data;
using Seeger.Web.UI.DataManagement;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Security
{
    public partial class RoleList : ListPageBase<Role>
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "RoleMgnt", "View");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override GridView GridView
        {
            get { return ListGrid; }
        }
    }
}