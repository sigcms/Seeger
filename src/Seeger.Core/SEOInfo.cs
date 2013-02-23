using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Seeger
{
    public class SEOInfo
    {
        private string _pageTitle = String.Empty;
        private string _metaKeywords = String.Empty;
        private string _metaDescription = String.Empty;

        public string PageTitle
        {
            get
            {
                return _pageTitle;
            }
            set
            {
                _pageTitle = value ?? String.Empty;
            }
        }

        public string MetaKeywords
        {
            get
            {
                return _metaKeywords;
            }
            set
            {
                _metaKeywords = value ?? String.Empty;
            }
        }

        public string MetaDescription
        {
            get
            {
                return _metaDescription;
            }
            set
            {
                _metaDescription = value ?? String.Empty;
            }
        }

        public SEOInfo()
        {
        }

        public SEOInfo(string pageTitle, string metaKeywords, string metaDescription)
        {
            _pageTitle = pageTitle ?? String.Empty;
            _metaKeywords = metaKeywords ?? String.Empty;
            _metaDescription = metaDescription ?? String.Empty;
        }

        public void Merge(string pageTitle, string metaKeywords, string metaDescription)
        {
            if (String.IsNullOrEmpty(PageTitle))
            {
                PageTitle = pageTitle ?? String.Empty;
            }
            if (String.IsNullOrEmpty(MetaKeywords))
            {
                MetaKeywords = metaKeywords ?? String.Empty;
            }
            if (String.IsNullOrEmpty(MetaDescription))
            {
                MetaDescription = metaDescription ?? String.Empty;
            }
        }

        public void Merge(SEOInfo other)
        {
            if (other != null)
            {
                Merge(other.PageTitle, other.MetaKeywords, other.MetaDescription);
            }
        }

        public SEOInfo Clone()
        {
            return new SEOInfo(PageTitle, MetaKeywords, MetaDescription);
        }
    }
}
