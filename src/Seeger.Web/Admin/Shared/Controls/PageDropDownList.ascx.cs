using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Caching;
using Seeger.Data;

namespace Seeger.Web.UI.Admin.Shared.Controls
{
    public partial class PageDropDownList : System.Web.UI.UserControl
    {
        public int SelectedPageId
        {
            get
            {
                int id;
                if (!Int32.TryParse(Drop.SelectedValue, out id))
                {
                    return 0;
                }

                return id;
            }
            set
            {
                Drop.SelectedValue = value.ToString();
            }
        }

        public bool AutoBind
        {
            get
            {
                return ViewState.TryGetValue<bool>("AutoBind", true);
            }
            set
            {
                ViewState["AutoBind"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (AutoBind)
            {
                Rebind();
            }
        }

        public void Rebind()
        {
            Bind(PageCache.From(NhSessionManager.GetCurrentSession()).RootPages, 0);
        }

        private void Bind(IEnumerable<PageItem> pages, int indent)
        {
            string prefix = String.Empty;
            if (indent > 0)
            {
                prefix = new String(Server.HtmlDecode("&nbsp;")[0], indent);
            }

            foreach (var page in pages)
            {
                var item = new ListItem(prefix + page.DisplayName, page.Id.ToString());
                Drop.Items.Add(item);

                Bind(page.Pages, indent + 1);
            }
        }
    }
}