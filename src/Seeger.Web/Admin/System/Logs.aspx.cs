using Seeger.Data;
using Seeger.Logging;
using Seeger.Web.UI.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.Admin._System
{
    public partial class Logs : AjaxGridPageBase
    {
        public override bool VerifyAccess(Seeger.Security.User user)
        {
            return user.HasPermission(null, "System", "Logs");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod, ScriptMethod]
        public static void ClearLogs()
        {
            Database.GetCurrentSession()
                    .CreateQuery("delete from " + typeof(LogEntry).FullName)
                    .ExecuteUpdate();
        }
    }
}