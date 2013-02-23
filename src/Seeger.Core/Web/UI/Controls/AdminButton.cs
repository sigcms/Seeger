using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public class AdminButton : System.Web.UI.WebControls.Button, IAdminControl
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

        public DenyMethod DenyMethod
        {
            get { return ViewState.TryGetValue<DenyMethod>("DenyMethod", DenyMethod.HideControl); }
            set { ViewState["DenyMethod"] = value; }
        }

        public override bool Visible
        {
            get
            {
                if (DenyMethod == DenyMethod.HideControl)
                {
                    return ValidateAccess();
                }
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }

        public override bool Enabled
        {
            get
            {
                if (DenyMethod == DenyMethod.DisableControl)
                {
                    return ValidateAccess();
                }
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
            }
        }

        private bool _authorized;
        private bool _validated;

        private bool ValidateAccess()
        {
            if (!_validated)
            {
                _authorized = this.ValidateAccess(AdministrationSession.Current.User);
                _validated = true;
            }
            return _authorized;
        }
    }
}
