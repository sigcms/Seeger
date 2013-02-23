using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Configuration;

using Seeger.Web;
using Seeger.Globalization;
using Seeger.Security;
using Seeger.Files;
using Seeger.Data;

namespace Seeger
{
    public class CmsConfiguration
    {
        private static CmsConfiguration _instance;

        public static CmsConfiguration Instance
        {
            get { return _instance; }
        }

        private CmsConfiguration()
        {
            PermissionGroups = new PermissionGroupCollection();
        }

        public string ConfigFilePath
        {
            get
            {
                return UrlUtility.Combine(InstallationInfo.InstallPath, "App_Data/Cms.config");
            }
        }

        public PermissionGroupCollection PermissionGroups { get; private set; }

        public TinyMceFontSettingCollection TinyMceFonts { get; private set; }

        public DevConfig DevConfig { get; private set; }

        #region Read Configuration

        public static void Initialize()
        {
            var config = new CmsConfiguration();
            
            var path = Server.MapPath(config.ConfigFilePath);
            var root = XDocument.Load(path).Root;

            var permissionsElement = root.Element("permissions");
            if (permissionsElement != null)
            {
                foreach (var each in permissionsElement.Elements())
                {
                    var group = PermissionGroup.From(each, null);
                    config.PermissionGroups.Add(group);
                }
            }

            var fileElement = root.Element("file");
            if (fileElement != null)
            {
                FileExplorer.ConfigureWith(fileElement);
            }

            var tinyMceElement = root.Element("tinymce");
            if (tinyMceElement != null)
            {
                var fontsElement = tinyMceElement.Element("fonts");
                if (fontsElement != null)
                {
                    config.TinyMceFonts = TinyMceFontSettingCollection.From(fontsElement);
                }
            }

            var devElement = root.Element("dev");
            if (devElement != null)
            {
                config.DevConfig = DevConfig.From(devElement);
            }

            _instance = config;
        }

        #endregion
    }
}
