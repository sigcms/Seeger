using Seeger.Web.UI.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Linq;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Security
{
    public partial class UserList_Grid : AjaxGridUserControlBase
    {
        public override void Bind(AjaxGridContext context)
        {
            var users = NhSession.Query<User>()
                                 .Where(u => !u.IsSuperAdmin)
                                 .OrderBy(u => u.UserName)
                                 .ThenByDescending(u => u.Id)
                                 .Paging(Pager.PageSize);

            List.DataSource = users.Page(context.PageIndex);
            List.DataBind();

            Pager.RecordCount = users.Count;
            Pager.PageIndex = context.PageIndex;
        }

        protected bool HasPermission(string group, string permission)
        {
            return AdminSession.Current.User.HasPermission(null, group, permission);
        }
    }
}