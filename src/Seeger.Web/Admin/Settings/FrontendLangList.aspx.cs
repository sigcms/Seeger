using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Data;
using Seeger.Licensing;
using Seeger.Security;
using Seeger.Web.UI.Grid;
using System.Web.Services;
using System.Web.Script.Services;

namespace Seeger.Web.UI.Admin.Settings
{
    public partial class FrontendLangList : AjaxGridPageBase
    {
        public override bool VerifyAccess(User user)
        {
            return LicensingService.CurrentLicense.CmsEdition.IsFeatureAvailable(Features.Multilingual)
                && user.HasPermission(null, "SiteSetting", "FrontendLanguage");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod, ScriptMethod]
        public static void Delete(string name)
        {
            var db = Database.GetCurrentSession();
            var lang = db.Get<FrontendLanguage>(name);
            db.Delete(lang);
            db.Commit();
        }
    }
}