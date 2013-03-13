using Seeger.Web.UI.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Linq;

namespace Seeger.Web.UI.Admin.Settings
{
    public partial class FrontendLangList_Grid : AjaxGridUserControlBase
    {
        public override void Bind(AjaxGridContext context)
        {
            var langs = NhSession.Query<FrontendLanguage>()
                                 .OrderBy(x => x.Name)
                                 .Paging(Pager.PageSize);

            List.DataSource = langs.Page(context.PageIndex);
            List.DataBind();

            Pager.RecordCount = langs.Count;
            Pager.PageIndex = context.PageIndex;
        }
    }
}