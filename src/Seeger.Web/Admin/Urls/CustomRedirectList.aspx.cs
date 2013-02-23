using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Seeger.Data;
using Seeger.Web.UI.DataManagement;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Urls
{
    public partial class CustomRedirectList : ListPageBase<CustomRedirect>
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "CustomRedirect", "View");
        }

        protected override bool AllowEditing
        {
            get
            {
                return CurrentUser.HasPermission(null, "CustomRedirect", "Edit");
            }
        }

        protected override bool AllowDeletion
        {
            get
            {
                return CurrentUser.HasPermission(null, "CustomRedirect", "Delete");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override string EditingPageUrl
        {
            get { return "CustomRedirectEdit.aspx"; }
        }

        protected override IQueryable<CustomRedirect> Filter(IQueryable<CustomRedirect> src)
        {
            return src.OrderBy(it => it.RedirectMode).ThenBy(it => it.Id);
        }

        protected override GridView GridView
        {
            get { return ListGrid; }
        }
    }
}