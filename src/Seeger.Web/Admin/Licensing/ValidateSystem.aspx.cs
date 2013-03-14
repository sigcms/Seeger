using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Seeger.Licensing;
using Seeger.Security;

namespace Seeger.Web.UI.Admin.Licensing
{
    public partial class ValidateSystem : AdminPageBase
    {
        private new Management Master
        {
            get
            {
                return (Management)base.Master;
            }
        }

        public override bool VerifyAccess(User user)
        {
            return user.IsSuperAdmin;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ActivateButton_Click(object sender, EventArgs e)
        {
            string licenseKey = LicenseKey.Text.Trim();

            if (String.IsNullOrEmpty(licenseKey) && !LicenseUpload.HasFile)
            {
                Master.ShowMessage(T("Licensing.PleaseEnterKeyOrUploadFile"), MessageType.Error);
                return;
            }

            if (!String.IsNullOrEmpty(licenseKey) && LicenseUpload.HasFile)
            {
                Master.ShowMessage(T("Licensing.EnterKeyOrUploadFileButNotBoth"), MessageType.Error);
                return;
            }

            if (!String.IsNullOrEmpty(licenseKey))
            {
                ValidateLicenseKey(licenseKey);
            }
            else
            {
                ValidateLicenseFile();
            }
        }

        private void ValidateLicenseFile()
        {
            string ext = Path.GetExtension(LicenseUpload.FileName);
            if (!ext.IgnoreCaseEquals(".licx"))
            {
                Master.ShowMessage(T("Licensing.LicenseFileTypeNotValid"), MessageType.Error);
            }
            else
            {
                string tempPath = Server.MapPath("~/App_Data/seeger-temp.licx");

                LicenseUpload.SaveAs(tempPath);

                if (LicensingService.ValidateLicense(tempPath).IsValid)
                {
                    File.Delete(LicensingService.LicenseFilePath);
                    File.Move(tempPath, LicensingService.LicenseFilePath);

                    LicensingService.ValidateCurrentLicense();

                    Response.Redirect("LicenseInfo.aspx");
                }
                else
                {
                    Master.ShowMessage(T("Licensing.LicenseFileNotValid"), MessageType.Error);
                }
            }
        }

        private void ValidateLicenseKey(string licenseKey)
        {
            if (LicensingService.ValidateLicenseKey(licenseKey).IsValid)
            {
                File.WriteAllText(LicensingService.LicenseFilePath, licenseKey);

                LicensingService.ValidateCurrentLicense();

                Response.Redirect("LicenseInfo.aspx");
            }
            else
            {
                Master.ShowMessage(T("Licensing.LicenseKeyNotValid"), MessageType.Error);
            }
        }

    }
}