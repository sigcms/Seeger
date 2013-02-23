using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Seeger.Licensing;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Licensing
{
    public partial class LicenseInfo : BackendPageBase
    {
        protected License License
        {
            get
            {
                return LicensingService.CurrentLicense;
            }
        }
        
        public override bool VerifyAccess(User user)
        {
            return user.IsSuperAdmin;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LicenseInfoHolder.Visible = License.IsValid;
        }
    }
}