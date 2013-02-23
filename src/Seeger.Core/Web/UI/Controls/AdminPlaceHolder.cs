using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public class AdminPlaceHolder : System.Web.UI.WebControls.PlaceHolder, IAdminControl
    {
        public bool RequireSuperAdmin
        {
            get { return ViewState.TryGetValue<bool>("RequireSuperAdmin", false); }
            set { ViewState["RequireSuperAdmin"] = value; }
        }

        public string Feature
        {
            get { return ViewState["Feature"] as String ?? String.Empty; }
            set { ViewState["Feature"] = value; }
        }

        public string Plugin
        {
            get { return ViewState["Plugin"] as String ?? String.Empty; }
            set { ViewState["Plugin"] = value; }
        }

        public string PermissionGroup
        {
            get { return ViewState["Function"] as String ?? String.Empty; }
            set { ViewState["Function"] = value; }
        }

        public string Permission
        {
            get { return ViewState["Operation"] as String ?? String.Empty; }
            set { ViewState["Operation"] = value; }
        }

        public override bool Visible
        {
            get
            {
                return this.ValidateAccess(AdministrationSession.Current.User);
            }
            set
            {
                base.Visible = value;
            }
        }
    }
}
