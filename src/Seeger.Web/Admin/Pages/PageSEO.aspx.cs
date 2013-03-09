using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using Seeger.Data;
using Seeger.Globalization;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Pages
{
    public partial class PageSEO : AdminPageBase
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "PageMgnt", "SEOSetting");
        }

        protected int PageId
        {
            get { return Convert.ToInt32(Request.QueryString["pageid"]); }
        }

        protected PageItem PageItem { get; private set; }

        protected string CultureCode
        {
            get
            {
                return Request.QueryString["culture"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PageItem = NhSession.Get<PageItem>(PageId);

            if (!IsPostBack)
            {
                BindForm();
            }
        }

        private void BindForm()
        {
            if (!String.IsNullOrEmpty(CultureCode))
            {
                var culture = CultureInfo.GetCultureInfo(CultureCode);

                LanguageHolder.Visible = true;
                LanguageName.Text = culture.DisplayName;

                PageTitle.Text = PageItem.GetLocalized(p => p.PageTitle);
                PageMetaKeywords.Text = PageItem.GetLocalized(p => p.MetaKeywords);
                PageMetaDescription.Text = PageItem.GetLocalized(p => p.MetaDescription);
            }
            else
            {
                PageTitle.Text = PageItem.PageTitle;
                PageMetaKeywords.Text = PageItem.MetaKeywords;
                PageMetaDescription.Text = PageItem.MetaDescription;
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(CultureCode))
            {
                var culture = CultureInfo.GetCultureInfo(CultureCode);
                
                PageItem.SetLocalized(p => p.PageTitle, PageTitle.Text);
                PageItem.SetLocalized(p => p.MetaKeywords, PageMetaKeywords.Text);
                PageItem.SetLocalized(p => p.MetaDescription, PageMetaDescription.Text);
            }
            else
            {
                PageItem.PageTitle = PageTitle.Text;
                PageItem.MetaKeywords = PageMetaKeywords.Text;
                PageItem.MetaDescription = PageMetaDescription.Text;
            }

            NhSession.Commit();

            ClientScript.RegisterClientScriptBlock(this.GetType(), "Success", "onSaved();", true);
        }
    }
}