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
    public partial class UserList : ListPageBase<Seeger.Security.User>, IRecordFilter<User>
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "UserMgnt", "View");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override IQueryable<User> Filter(IQueryable<User> src)
        {
            return src.Where(it => !it.IsSuperAdmin).OrderByDescending(it => it.Id);
        }

        protected override GridView GridView
        {
            get { return ListGrid; }
        }

        public bool IsVisible(User record)
        {
            return true;
        }

        public bool IsEditable(User record)
        {
            return record.Id != CurrentUser.Id;
        }

        public bool IsDeletable(User record)
        {
            return record.Id != CurrentUser.Id;
        }
    }
}