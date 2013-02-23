using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;

using Seeger.Data;
using Seeger.Collections;
using Seeger.Globalization;
using Seeger.Licensing;

namespace Seeger
{
    public class FrontendSettings
    {
        private GlobalSettingManager _manager;
        private const string Prefix = "Seeger.Frontend.";

        private readonly string[] _supportedPageExtensions = { ".aspx", ".html", ".htm" };

        public IEnumerable<string> SupportedPageExtensions
        {
            get { return _supportedPageExtensions; }
        }

        public string PageExtension
        {
            get
            {
                return _manager.TryGetValue(Prefix + "PageExtension", _supportedPageExtensions[0]);
            }
            set
            {
                _manager.SetValue(Prefix + "PageExtension", value);
            }
        }

        public bool Multilingual
        {
            get
            {
                if (!LicensingService.CurrentLicense.CmsEdition.IsFeatureAvailable(Features.Multilingual))
                {
                    return false;
                }

                return _manager.TryGetValue<bool>(Prefix + "Multilingual", false);
            }
            set
            {
                _manager.SetValue(Prefix + "Multilingual", value.ToString());
            }
        }

        public string DefaultLanguage
        {
            get
            {
                return _manager.GetValue(Prefix + "DefaultLanguage");
            }
            set
            {
                _manager.SetValue(Prefix + "DefaultLanguage", value);
            }
        }

        public bool IsWebsiteOffline
        {
            get
            {
                return _manager.TryGetValue<bool>(Prefix + "IsWebsiteOffline", false);
            }
            set
            {
                _manager.SetValue(Prefix + "IsWebsiteOffline", value.ToString());
            }
        }

        public int OfflinePageId
        {
            get
            {
                return _manager.TryGetValue(Prefix + "OfflinePageId", 0);
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                _manager.SetValue(Prefix + "OfflinePageId", value.ToString());
            }
        }

        public FrontendSettings(GlobalSettingManager manager)
        {
            _manager = manager;
        }
    }
}
