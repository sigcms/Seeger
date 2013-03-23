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

        public FrontendSettings(GlobalSettingManager manager)
        {
            _manager = manager;
        }
    }
}
