using Seeger.Web.UI.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Linq;

namespace Seeger.Web.UI.Admin.Urls
{
    public partial class CustomRedirectList_Grid : AjaxGridUserControlBase
    {
        public override void Bind(AjaxGridContext context)
        {
            var redirects = NhSession.Query<CustomRedirect>()
                                     .OrderByDescending(x => x.Id)
                                     .Paging(Pager.PageSize);

            List.DataSource = redirects.Page(context.PageIndex);
            List.DataBind();

            Pager.RecordCount = redirects.Count;
            Pager.PageIndex = context.PageIndex;
        }

        protected bool HasPermission(string groupName, string permission)
        {
            return AdminSession.Current.User.HasPermission(null, groupName, permission);
        }
    }
}