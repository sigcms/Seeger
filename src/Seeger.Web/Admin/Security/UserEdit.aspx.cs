using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Linq;
using Seeger.Data;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Security
{
    public partial class UserEdit : AdminPageBase
    {
        protected int UserId
        {
            get
            {
                return Request.QueryString.TryGetValue<int>("id", 0);
            }
        }

        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "UserMgnt", (UserId == 0) ? "Add" : "Edit");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (UserId > 0)
                {
                    InitView(NhSession.Get<User>(UserId));
                }
                else
                {
                    InitView(new User());
                }
            }
        }

        public void InitView(User entity)
        {
            RoleList.DataSource = NhSession.Query<Role>().OrderBy(it => it.Id);
            RoleList.DataBind();

            if (UserId > 0)
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

        public void UpdateObject(User entity)
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
            if (UserId == 0)
            {
                e.IsValid = NhSession.Query<User>().Any(u => u.UserName == e.Value) == false;
            }
            else
            {
                e.IsValid = NhSession.Query<User>().Any(u => u.Id != UserId && u.UserName == e.Value) == false;
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            if (UserId > 0)
            {
                var user = NhSession.Get<User>(UserId);
                UpdateObject(user);
            }
            else
            {
                var user = new User();
                UpdateObject(user);
                NhSession.Save(user);
            }

            NhSession.Commit();

            Response.Redirect("UserList.aspx");
        }
    }
}