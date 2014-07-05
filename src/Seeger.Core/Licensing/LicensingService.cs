using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Seeger.Licensing
{
    public class LicensingService
    {
        public static readonly string LicenseFilePath;

        static LicensingService()
        {
            CurrentLicense = License.InvalidLicense;
            LicenseFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "seeger.licx");
        }

        public static License CurrentLicense { get; private set; }

        private static readonly object _validationLock = new object();

        public static void ValidateCurrentLicense()
        {
            lock (_validationLock)
            {
                CurrentLicense = ValidateLicense(LicenseFilePath);
            }
        }

        public static License ValidateLicense(string licensePath)
        {
            return SeegerLicenseValidator.ValidateLicense(licensePath);
        }

        public static License ValidateLicenseKey(string licenseKey)
        {
            string tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "seeger-temp.licx");

            License license = License.InvalidLicense;

            try
            {
                File.WriteAllText(tempPath, licenseKey);

                license = ValidateLicense(tempPath);
            }
            finally
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
            }

            return license;
        }
    }
}
