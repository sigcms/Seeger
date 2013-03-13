using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Data;
using Seeger.Security;
using System.Web.Services;
using System.Web.Script.Services;
using Seeger.Web.UI.Grid;

namespace Seeger.Web.UI.Admin.Security
{
    public partial class UserList : AjaxGridPageBase
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "UserMgnt", "View");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod, ScriptMethod]
        public static void Delete(int id)
        {
            var db = Database.GetCurrentSession();
            var user = db.Get<User>(id);
            db.Delete(user);
            db.Commit();
        }
    }
}