using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Linq;
using Seeger.Data;
using Seeger.Web.UI.DataManagement;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Security
{
    public partial class UserEdit : DetailPageBase<Seeger.Security.User>
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "UserMgnt", (FormState == UI.FormState.AddItem) ? "Add" : "Edit");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override object CreateKey(string keyStringValue)
        {
            return Convert.ToInt32(keyStringValue);
        }

        protected override void OnSubmitted()
        {
            Response.Redirect("UserList.aspx");
        }

        protected override void BindSubmitEventHandler(EventHandler handler)
        {
            SubmitButton.Click += handler;
        }

        public override void InitView(User entity)
        {
            RoleList.DataSource = NhSession.Query<Role>().OrderBy(it => it.Id);
            RoleList.DataBind();

            if (FormState == UI.FormState.EditItem)
            {
                PasswordRequired.Enabled = false;
            }

            UserName.Text = entity.UserName;
            Nick.Text = entity.Nick;
            Email.Text = entity.Email;
            Password.Text = entity.Password;

            foreach (var role in entity.Roles)
            {
                foreach (ListItem item in RoleList.Items)
                {
                    if (Convert.ToInt32(item.Value) == role.Id)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        public override void UpdateObject(User entity)
        {
            entity.UserName = UserName.Text.Trim();
            entity.Nick = Nick.Text.Trim();
            entity.Email = Email.Text.Trim();

            if (Password.Text.Length > 0)
            {
                entity.UpdatePassword(Password.Text);
            }

            var roleIds = new List<int>();

            foreach (ListItem item in RoleList.Items)
            {
                if (item.Selected)
                    roleIds.Add(Convert.ToInt32(item.Value));
            }

            var roles = NhSession.Query<Role>().Where(it => roleIds.Contains(it.Id));

            entity.Roles.Clear();

            foreach (var role in roles)
            {
                entity.Roles.Add(role);
            }
        }

        protected void UserNameDuplicateValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (FormState == UI.FormState.AddItem)
            {
                e.IsValid = NhSession.Query<User>().Any(u => u.UserName == e.Value) == false;
            }
            else
            {
                e.IsValid = NhSession.Query<User>().Any(u => u.Id != Entity.Id && u.UserName == e.Value) == false;
            }
        }
    }
}