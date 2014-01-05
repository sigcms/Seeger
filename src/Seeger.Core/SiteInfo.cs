using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Seeger.Data.Mapping;
using Seeger.Data.Mapping.Attributes;

namespace Seeger
{
    [Entity, Cache]
    public class SiteInfo
    {
        [EntityKey, Id]
        public string Culture { get; set; }
        public string SiteTitle { get; set; }
        public string SiteSubtitle { get; set; }
        public string LogoFilePath { get; set; }
        public string Copyright { get; set; }
        public string MiiBeiAnNumber { get; set; }
        public SEOInfo SEOInfo { get; private set; }

        protected SiteInfo() { }

        public SiteInfo(string culture)
        {
            Require.NotNullOrEmpty(culture, "culture");

            Culture = culture;
            SiteTitle = String.Empty;
            SiteSubtitle = String.Empty;
            LogoFilePath = String.Empty;
            Copyright = String.Empty;
            MiiBeiAnNumber = String.Empty;

            SEOInfo = new SEOInfo();
        }

        public void UpdateWith(SiteInfo info)
        {
            Require.NotNull(info, "info");

            SiteTitle = info.SiteTitle;
            SiteSubtitle = info.SiteSubtitle;
            LogoFilePath = info.LogoFilePath;
            Copyright = info.Copyright;
            MiiBeiAnNumber = info.MiiBeiAnNumber;
            SEOInfo.PageTitle = info.SEOInfo.PageTitle;
            SEOInfo.MetaKeywords = info.SEOInfo.MetaKeywords;
            SEOInfo.MetaDescription = info.SEOInfo.MetaDescription;
        }
    }
}
