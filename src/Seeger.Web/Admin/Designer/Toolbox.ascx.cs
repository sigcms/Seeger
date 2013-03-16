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
    public partial class Toolbox : AdminUserControlBase
    {
        protected string PageCulture
        {
            get
            {
                return Request.QueryString["culture"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCultureList();
            }
        }

        private void BindCultureList()
        {
            SelectLanguageHolder.Visible = FrontendSettings.Multilingual;

            if (FrontendSettings.Multilingual)
            {
                CultureList.DataSource = FrontendLanguageCache.From(NhSession).Languages;
                CultureList.DataBind();

                CultureList.SelectedValue = PageCulture;
            }
        }

        protected override string T(string key)
        {
            return ResourcesFolder.Global.GetValue(key, AdminSession.Current.UICulture);
        }
    }
}