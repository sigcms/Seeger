using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using Seeger.Globalization;
using Seeger.Caching;

namespace Seeger.Web.UI.Admin.Designer
{
    public partial class DesignerElement : System.Web.UI.UserControl
    {
        private PageDesignerBase DesignerPage
        {
            get
            {
                PageDesignerBase page = base.Page as PageDesignerBase;
                if (page == null)
                    throw new InvalidOperationException(
                        String.Format("Designer.master can only be used by pages which inherit from type '{0}'.", typeof(PageDesignerBase).FullName));

                return page;
            }
        }

        public PageItem PageItem
        {
            get
            {
                return DesignerPage.PageItem;
            }
        }

        protected int PageId
        {
            get { return PageItem.Id; }
        }

        protected string PageLiveUrl
        {
            get
            {
                return FrontendEnvironment.GetPageUrl(Request.QueryString["page-culture"], PageItem);
            }
        }

        protected string TemplateName
        {
            get { return PageItem.Layout.Template.Name; }
        }

        protected string LayoutName
        {
            get { return PageItem.Layout.Name; }
        }

        protected string T(string key)
        {
            return ResourcesFolder.Global.GetValue(key, CultureInfo.CurrentUICulture);
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}