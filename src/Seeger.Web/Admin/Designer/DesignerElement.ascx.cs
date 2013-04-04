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
        private LayoutPageBase LayoutPage
        {
            get
            {
                return (LayoutPageBase)Page;
            }
        }

        public PageItem PageItem
        {
            get
            {
                return LayoutPage.PageItem;
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
                return FrontendEnvironment.GetPageUrl(Request.QueryString["culture"], PageItem);
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
            return ResourceFolder.Global.GetValue(key, CultureInfo.CurrentUICulture);
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}