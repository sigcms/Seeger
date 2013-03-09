using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public class AdminTextBox : System.Web.UI.WebControls.TextBox, IAdminControl
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

        public override bool ReadOnly
        {
            get
            {
                return this.ValidateAccess(AdminSession.Current.User);
            }
            set
            {
                base.ReadOnly = value;
            }
        }
    }
}
