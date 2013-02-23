using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using Seeger.Caching;
using Seeger.Globalization;
using Seeger.Data;

namespace Seeger.Web.UI.Templates.Default.Layouts
{
    public partial class Front : System.Web.UI.MasterPage
    {
        private CultureInfo _pageCulture;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitPageCulture();
                BindLogo();
                BindSubtitle();
                BindMenu();
                BindLanguageSwitch();
                BindCopyright();
                BindPoweredBy();
                BindMiiBeiAnNumber();
            }
        }

        private void BindLogo()
        {
            var defaultSiteInfo = GlobalSettingManager.Instance.DefaultSiteInfo;

            string logoPath = defaultSiteInfo.LogoFilePath;
            string siteTitle = defaultSiteInfo.SiteTitle;

            if (GlobalSettingManager.Instance.FrontendSettings.Multilingual)
            {
                var siteInfo = SiteInfoCache.From(NhSessionManager.GetCurrentSession()).GetSiteInfo(_pageCulture);
                if (siteInfo != null)
                {
                    if (!String.IsNullOrEmpty(siteInfo.SiteTitle))
                    {
                        siteTitle = siteInfo.SiteTitle;
                    }
                    if (!String.IsNullOrEmpty(siteInfo.LogoFilePath))
                    {
                        logoPath = siteInfo.LogoFilePath;
                    }
                }
            }

            string rootUrl = FrontendEnvironment.GetRootUrl(_pageCulture);
            string html = null;

            if (!String.IsNullOrEmpty(logoPath))
            {
                html = String.Format("<div class='logo'><a href='{0}' title='{1}'><img src='{2}' alt='{1}' /></a></div>", rootUrl, siteTitle, logoPath);
            }
            else
            {
                html = String.Format("<div class='logo'><h1 class='site-title'><a href='{0}' title='{1}'>{1}</a></h1></div>", rootUrl, siteTitle);
            }

            Logo.Text = html;
        }

        private void BindSubtitle()
        {
            string subtitle = GlobalSettingManager.Instance.DefaultSiteInfo.SiteSubtitle;

            if (GlobalSettingManager.Instance.FrontendSettings.Multilingual)
            {
                var siteInfo = SiteInfoCache.From(NhSessionManager.GetCurrentSession()).GetSiteInfo(_pageCulture);
                if (siteInfo != null && !String.IsNullOrEmpty(siteInfo.SiteSubtitle))
                {
                    subtitle = siteInfo.SiteSubtitle;
                }
            }

            if (!String.IsNullOrEmpty(subtitle))
            {
                Subtitle.Text = "<h2 class='site-subtitle'>" + subtitle + "</h2>";
            }
        }

        private void InitPageCulture()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["page-culture"]))
            {
                _pageCulture = CultureInfo.GetCultureInfo(Request.QueryString["page-culture"]);
            }

            if (_pageCulture == null)
            {
                _pageCulture = CultureInfo.CurrentCulture;
            }
        }

        private void BindLanguageSwitch()
        {
            if (GlobalSettingManager.Instance.FrontendSettings.Multilingual)
            {
                LanguageSwitch.Text = new LanguageSwitchRenderer(_pageCulture).Render();
            }
        }

        private void BindMenu()
        {
            var page = Page as LayoutPageBase;

            if (page != null)
            {
                Menu.Text = new MenuRenderer(_pageCulture).Render(page.PageItem.Id);
            }
            else
            {
                Menu.Text = new MenuRenderer(_pageCulture).Render();
            }
        }

        private void BindCopyright()
        {
            string copyright = GlobalSettingManager.Instance.DefaultSiteInfo.Copyright;

            if (GlobalSettingManager.Instance.FrontendSettings.Multilingual)
            {
                var siteInfo = SiteInfoCache.From(NhSessionManager.GetCurrentSession()).GetSiteInfo(_pageCulture);
                if (siteInfo != null && !String.IsNullOrEmpty(siteInfo.Copyright))
                {
                    copyright = siteInfo.Copyright;
                }
            }

            Copyright.Text = copyright;
        }

        private void BindPoweredBy()
        {
            string format = "Powered by <a href=\"" + SeegerUrls.Homepage + "\" target=\"_blank\" title=\"{0}\">{0}</a>.";
            string cmsName = ResourcesFolder.Global.GetValue("Seeger.ShortName", _pageCulture) ?? "西格CMS";

            PoweredBy.Text = String.Format(format, cmsName);
        }

        private void BindMiiBeiAnNumber()
        {
            string beiAnNumber = GlobalSettingManager.Instance.DefaultSiteInfo.MiiBeiAnNumber;

            if (GlobalSettingManager.Instance.FrontendSettings.Multilingual)
            {
                var siteInfo = SiteInfoCache.From(NhSessionManager.GetCurrentSession()).GetSiteInfo(_pageCulture);
                if (siteInfo != null)
                {
                    beiAnNumber = siteInfo.MiiBeiAnNumber;
                }
            }

            if (!String.IsNullOrEmpty(beiAnNumber))
            {
                MiiBeiAnNumber.Text = String.Format("<a href='http://www.miibeian.gov.cn/' target='_blank'>{0}</a>", beiAnNumber);
            }
        }
    }
}