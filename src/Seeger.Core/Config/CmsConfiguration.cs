using Seeger.Files;
using Seeger.Web;
using System.Xml.Linq;

namespace Seeger.Config
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
        }

        public string ConfigFilePath
        {
            get
            {
                return UrlUtil.Combine("/App_Data/cms.config");
            }
        }

        public SecurityConfig Security { get; private set; }

        public TinyMceFontConfigCollection TinyMceFonts { get; private set; }

        public DevConfig DevConfig { get; private set; }

        #region Read Configuration

        public static void Initialize()
        {
            var config = new CmsConfiguration();
            
            var path = Server.MapPath(config.ConfigFilePath);
            var root = XDocument.Load(path).Root;

            var securityElement = root.Element("security");
            if (securityElement != null)
            {
                config.Security = SecurityConfig.From(securityElement);
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
                    config.TinyMceFonts = TinyMceFontConfigCollection.From(fontsElement);
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
