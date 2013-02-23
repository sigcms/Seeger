using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino.Licensing;
using System.Collections.ObjectModel;

namespace Seeger.Licensing
{
    public sealed class License
    {
        public static readonly License InvalidLicense;

        public Guid Id { get; private set; }
        public string UserName { get; private set; }
        public bool IsTrial { get; private set; }
        public DateTime ExpirationDate { get; private set; }

        public bool NeverExpire
        {
            get
            {
                return ExpirationDate >= DateTime.MaxValue;
            }
        }

        public Version CmsVersion { get; private set; }
        public Edition CmsEdition { get; private set; }

        public IEnumerable<string> SupportedDomains { get; private set; }

        public bool IsValid
        {
            get
            {
                return !Id.Equals(Guid.Empty)
                    && ExpirationDate > DateTime.Now
                    && CmsVersion.Major == SeegerInfo.Version.Major;
            }
        }

        public License(Guid id,
                       string userName,
                       Version cmsVersion,
                       Edition cmsEdition,
                       DateTime expirationDate,
                       bool isTrial,
                       string supportedDomains)
        {
            if (cmsVersion == null)
                throw new ArgumentNullException("cmsVersion");

            if (cmsEdition == null)
                throw new ArgumentNullException("cmsEdition");

            Id = id;
            UserName = userName ?? String.Empty;
            CmsVersion = cmsVersion;
            CmsEdition = cmsEdition;
            ExpirationDate = expirationDate;
            IsTrial = isTrial;
            
            var domains = new List<string>();

            if (!String.IsNullOrEmpty(supportedDomains))
            {
                domains.AddRange(supportedDomains.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(it => Domain.Remove3W(it)));
            }

            SupportedDomains = new ReadOnlyCollection<string>(domains);
        }

        static License()
        {
            InvalidLicense = new License(
                Guid.Empty,
                String.Empty,
                new Version(0, 0, 0, 0),
                InvalidEdition.Instance,
                DateTime.MinValue,
                true,
                null);
        }

        public bool IsFeatureAvailable(string feature)
        {
            return CmsEdition.IsFeatureAvailable(feature);
        }

        public bool IsDomainLicensed(string domain)
        {
            // Real visitors will never enter ip address
            if (Domain.IsIPv4Address(domain) || Domain.IsLocalHost(domain))
            {
                return true;
            }

            return SupportedDomains.Contains(Domain.Remove3W(domain));
        }

        internal static License CreateFrom(LicenseValidator validator)
        {
            return new License
            (
                validator.UserId,
                validator.Name,
                new Version(validator.LicenseAttributes[LicenseProperty.CmsVersion]),
                Editions.GetEdition(validator.LicenseAttributes[LicenseProperty.CmsEdition]),
                validator.ExpirationDate,
                validator.LicenseType == LicenseType.Trial,
                validator.LicenseAttributes[LicenseProperty.SupportedDomains]
            );
        }
    }
}
