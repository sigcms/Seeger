using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeger.Files;
using System.IO;
using Seeger.Security;
using Seeger.Caching;

namespace Seeger.Web.UI.Admin.Settings
{
    public partial class SiteInfoEdit : AdminPageBase
    {
        public override bool VerifyAccess(User user)
        {
            return user.HasPermission(null, "SiteSetting", "SiteInfo");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitView();
            }
        }

        private void InitView()
        {
            if (!IsPostBack && FrontendSettings.Multilingual)
            {
                BindLanguageList();
            }

            string culture = LanguageList.SelectedValue;

            if (String.IsNullOrEmpty(culture))
            {
                BindViewWith(GlobalSettingManager.Instance.DefaultSiteInfo);
            }
            else
            {
                var info = NhSession.Get<SiteInfo>(culture);

                if (info != null)
                {
                    BindViewWith(info);
                }
            }
        }

        private void BindViewWith(SiteInfo siteInfo)
        {
            SiteTitle.Text = siteInfo.SiteTitle;
            SiteSubtitle.Text = siteInfo.SiteSubtitle;
            Copyright.Text = siteInfo.Copyright;
            MiiBeiAnNumber.Text = siteInfo.MiiBeiAnNumber;

            PageTitle.Text = siteInfo.SEOInfo.PageTitle;
            PageMetaKeywords.Text = siteInfo.SEOInfo.MetaKeywords;
            PageMetaDescription.Text = siteInfo.SEOInfo.MetaDescription;

            BindLogo(siteInfo.LogoFilePath);
        }

        private void BindViewWith(DefaultSiteInfo siteInfo)
        {
            SiteTitle.Text = siteInfo.SiteTitle;
            SiteSubtitle.Text = siteInfo.SiteSubtitle;
            Copyright.Text = siteInfo.Copyright;
            MiiBeiAnNumber.Text = siteInfo.MiiBeiAnNumber;

            PageTitle.Text = siteInfo.PageTitle;
            PageMetaKeywords.Text = siteInfo.MetaKeywords;
            PageMetaDescription.Text = siteInfo.MetaDescription;

            BindLogo(siteInfo.LogoFilePath);
        }

        private void BindLogo(string logoPath)
        {
            if (!String.IsNullOrEmpty(logoPath))
            {
                LogoPreviewHolder.Visible = true;
                LogoPreview.ImageUrl = logoPath;
            }
            else
            {
                LogoPreviewHolder.Visible = false;
            }
        }

        private void ClearForm()
        {
            ((Management)Master).HideMessage();

            SiteTitle.Text = String.Empty;
            SiteSubtitle.Text = String.Empty;
            Copyright.Text = String.Empty;

            PageTitle.Text = String.Empty;
            PageMetaKeywords.Text = String.Empty;
            PageMetaDescription.Text = String.Empty;

            LogoPreviewHolder.Visible = false;
        }

        private void BindLanguageList()
        {
            LanguageHodler.Visible = true;
            LanguageList.DataSource = FrontendLanguageCache.From(NhSession).Languages;
            LanguageList.DataBind();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string culture = LanguageList.SelectedValue;

            if (String.IsNullOrEmpty(culture))
            {
                var info = GlobalSettingManager.Instance.DefaultSiteInfo;

                info.SiteTitle = SiteTitle.Text;
                info.SiteSubtitle = SiteSubtitle.Text;
                info.Copyright = Copyright.Text;
                info.MiiBeiAnNumber = MiiBeiAnNumber.Text.Trim();

                info.PageTitle = PageTitle.Text.Trim();
                info.MetaKeywords = PageMetaKeywords.Text.Trim();
                info.MetaDescription = PageMetaDescription.Text.Trim();

                if (DeleteLogo.Checked)
                {
                    DeleteOldLogo(info.LogoFilePath);
                    info.LogoFilePath = String.Empty;
                }

                if (LogoUpload.HasFile)
                {
                    info.LogoFilePath = SaveLogo(info.LogoFilePath, null);
                }

                GlobalSettingManager.Instance.SubmitChanges();

                BindLogo(info.LogoFilePath);
            }
            else
            {
                var info = NhSession.Get<SiteInfo>(culture);

                if (info == null)
                {
                    info = new SiteInfo(culture);
                    NhSession.Save(info);
                }

                info.SiteTitle = SiteTitle.Text;
                info.SiteSubtitle = SiteSubtitle.Text;
                info.Copyright = Copyright.Text;
                info.MiiBeiAnNumber = MiiBeiAnNumber.Text.Trim();

                info.SEOInfo.PageTitle = PageTitle.Text.Trim();
                info.SEOInfo.MetaKeywords = PageMetaKeywords.Text.Trim();
                info.SEOInfo.MetaDescription = PageMetaDescription.Text.Trim();

                if (DeleteLogo.Checked)
                {
                    DeleteOldLogo(info.LogoFilePath);
                    info.LogoFilePath = String.Empty;
                }

                if (LogoUpload.HasFile)
                {
                    info.LogoFilePath = SaveLogo(info.LogoFilePath, culture);
                }

                NhSession.Commit();

                BindLogo(info.LogoFilePath);
            }

            ((Management)Master).ShowMessage(Localize("Message.SaveSuccess"), MessageType.Success);
        }

        private void DeleteOldLogo(string oldLogoPath)
        {
            if (!String.IsNullOrEmpty(oldLogoPath))
            {
                string path = Server.MapPath(oldLogoPath);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        private string SaveLogo(string oldLogoPath, string culture)
        {
            if (!LogoUpload.HasFile)
            {
                return String.Empty;
            }

            if (!String.IsNullOrEmpty(oldLogoPath))
            {
                string path = Server.MapPath(oldLogoPath);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            string fileName = "logo";

            if (!String.IsNullOrEmpty(culture))
            {
                fileName = "logo-" + culture;
            }

            fileName += Path.GetExtension(LogoUpload.FileName);

            fileName = FileExplorer.CalculateFinalName("/Files", fileName);

            string virtualPath = "/Files/" + fileName;

            LogoUpload.SaveAs(Server.MapPath(virtualPath));

            return virtualPath;
        }

        protected void LanguageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearForm();
            InitView();
        }

        protected void LogoValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (e.Value.Length > 0)
            {
                e.IsValid = FileType.IsImageFile(e.Value);
            }
        }
    }
}