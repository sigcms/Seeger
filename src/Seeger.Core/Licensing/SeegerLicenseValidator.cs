using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Seeger.Licensing
{
    public class SeegerLicenseValidator
    {
        public static License ValidateLicense(string licensePath)
        {
            License license = License.InvalidLicense;

            if (File.Exists(licensePath))
            {
                var validator = new Rhino.Licensing.LicenseValidator(LoadPublicKey(), licensePath);
                try
                {
                    validator.AssertValidLicense();

                    license = License.CreateFrom(validator);
                }
                catch (Rhino.Licensing.RhinoLicensingException)
                {
                    license = License.InvalidLicense;
                }
            }

            return license;
        }

        private static string LoadPublicKey()
        {
            using (var reader = new StreamReader(typeof(SeegerLicenseValidator).Assembly.GetManifestResourceStream("Seeger.Licensing.PublicKey.xml")))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
