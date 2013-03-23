using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Seeger.Data;

namespace Seeger
{
    public class DefaultSiteInfo
    {
        private GlobalSettingManager _manager;
        private const string Prefix = "Seeger.SiteInfo.";

        public string SiteTitle
        {
            get
            {
                return _manager.GetValue(Prefix + "SiteTitle");
            }
            set
            {
                _manager.SetValue(Prefix + "SiteTitle", value);
            }
        }

        public string SiteSubtitle
        {
            get
            {
                return _manager.GetValue(Prefix + "SiteSubtitle");
            }
            set
            {
                _manager.SetValue(Prefix + "SiteSubtitle", value);
            }
        }

        public string LogoFilePath
        {
            get
            {
                return _manager.GetValue(Prefix + "LogoFilePath");
            }
            set
            {
                _manager.SetValue(Prefix + "LogoFilePath", value);
            }
        }

        public string Copyright
        {
            get
            {
                return _manager.GetValue(Prefix + "Copyright");
            }
            set
            {
                _manager.SetValue(Prefix + "Copyright", value);
            }
        }

        public string MiiBeiAnNumber
        {
            get
            {
                return _manager.GetValue(Prefix + "MiiBeiAnNumber");
            }
            set
            {
                _manager.SetValue(Prefix + "MiiBeiAnNumber", value);
            }
        }

        public string PageTitle
        {
            get
            {
                return _manager.GetValue(Prefix + "PageTitle");
            }
            set
            {
                _manager.SetValue(Prefix + "PageTitle", value);
            }
        }

        public string MetaKeywords
        {
            get
            {
                return _manager.GetValue(Prefix + "MetaKeywords");
            }
            set
            {
                _manager.SetValue(Prefix + "MetaKeywords", value);
            }
        }

        public string MetaDescription
        {
            get
            {
                return _manager.GetValue(Prefix + "Key_MetaDescription");
            }
            set
            {
                _manager.SetValue(Prefix + "Key_MetaDescription", value);
            }
        }

        public DefaultSiteInfo(GlobalSettingManager manager)
        {
            Require.NotNull(manager, "manager");

            _manager = manager;
        }
    }
}