using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Data;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Security
{
    public partial class RoleEdit : AdminPageBase
    {
        protected int RoleId
        {
            get
            {
                return Request.QueryString.TryGetValue<int>("id", 0);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (RoleId > 0)
                {
                    InitView(NhSession.Get<Role>(RoleId));
                }
                else
                {
                    InitView(new Role());
                }
            }
        }

        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "RoleMgnt", RoleId == 0 ? "Add" : "Edit");
        }

        public void InitView(Role entity)
        {
            Name.Text = entity.Name;
            Privileges.Bind(entity);
        }

        public void UpdateObject(Role entity)
        {
            entity.Name = Name.Text.Trim();
            Privileges.Update(entity);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            if (RoleId > 0)
            {
                var role = NhSession.Get<Role>(RoleId);
                UpdateObject(role);
            }
            else
            {
                var role = new Role();
                UpdateObject(role);
                NhSession.Save(role);
            }

            NhSession.Commit();

            Response.Redirect("RoleList.aspx");
        }
    }
}