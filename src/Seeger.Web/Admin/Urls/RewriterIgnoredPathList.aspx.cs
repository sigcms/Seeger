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
    public partial class ReservedPathList : ListPageBase<RewriterIgnoredPath>, IRecordFilter<RewriterIgnoredPath>
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "RewriterIgnoredPath", "View");
        }

        protected override bool AllowEditing
        {
            get
            {
                return CurrentUser.HasPermission(null, "RewriterIgnoredPath", "Edit");
            }
        }

        protected override bool AllowDeletion
        {
            get
            {
                return CurrentUser.HasPermission(null, "RewriterIgnoredPath", "Delete");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override IQueryable<RewriterIgnoredPath> Filter(IQueryable<RewriterIgnoredPath> src)
        {
            return src.OrderByDescending(it => it.Reserved).ThenBy(it => it.Id);
        }

        protected override GridView GridView
        {
            get { return ListGrid; }
        }

        public bool IsVisible(RewriterIgnoredPath record)
        {
            return true;
        }

        public bool IsEditable(RewriterIgnoredPath record)
        {
            return record.Reserved == false;
        }

        public bool IsDeletable(RewriterIgnoredPath record)
        {
            return record.Reserved == false;
        }
    }
}