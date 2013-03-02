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
    public partial class RoleList_Grid : AjaxGridUserControlBase
    {
        public override void Bind(AjaxGridBindingContext context)
        {
            var roles = NhSession.Query<Role>().OrderBy(x => x.Name).ThenBy(x => x.Id).Paging(Pager.PageSize);

            List.DataSource = roles.Page(context.PageIndex);
            List.DataBind();

            Pager.RecordCount = roles.Count;
            Pager.PageIndex = context.PageIndex;
        }
    }
}