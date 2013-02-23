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
    public partial class RoleEdit : DetailPageBase<Role>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "RoleMgnt", (FormState == UI.FormState.AddItem) ? "Add" : "Edit");
        }

        protected override object CreateKey(string keyStringValue)
        {
            return Convert.ToInt32(keyStringValue);
        }

        protected override void OnSubmitted()
        {
            Response.Redirect("RoleList.aspx");
        }

        protected override void BindSubmitEventHandler(EventHandler handler)
        {
            SubmitButton.Click += handler;
        }

        public override void InitView(Role entity)
        {
            Name.Text = entity.Name;
            Privileges.Bind(entity);
        }

        public override void UpdateObject(Role entity)
        {
            entity.Name = Name.Text.Trim();
            Privileges.Update(entity);
        }
    }
}