using Seeger.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using Seeger.Globalization;

namespace Seeger.Templates.Loaders
{
    public class DefaultTemplateLoader : ITemplateLoader
    {
        public Template Load(string templateName)
        {
            var template = new Template(templateName);
            Configure(template);
            return template;
        }

        private void Configure(Template template)
        {
            string path = Server.MapPath(UrlUtil.Combine(template.VirtualPath, "config.config"));
            if (File.Exists(path))
            {
                var xml = XDocument.Load(path).Root;
                var displayName = xml.ChildElementValue("display-name");

                if (!String.IsNullOrEmpty(displayName))
                {
                    template.SetDisplayName(new LocalizableText(displayName));
                }
            }
        }
    }
}
