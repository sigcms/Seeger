using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Seeger.Security;
using Seeger.Caching;

namespace Seeger.Web.UI.Admin.Pages
{
    public partial class PageList : BackendPageBase
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "PageMgnt", "View");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected bool Multilingual
        {
            get { return FrontendSettings.Multilingual; }
        }

        protected string RenderPageTree()
        {
            return TreeNode.RenderHtml(TreeNode.FromPageCache(PageCache.From(NhSession)));
        }

        protected string GetNoPageHintPanelStyle()
        {
            if (PageCache.From(NhSession).HasPage)
            {
                return "display:none";
            }

            return String.Empty;
        }
    }
}