using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

using Seeger.Web;
using Seeger.Globalization;
using Seeger.Templates.Parsers;

namespace Seeger.Templates
{
    public class Layout
    {
        public string Name { get; private set; }

        public string FullName
        {
            get { return Template.Name + "." + Name; }
        }

        public LocalizableText DisplayName { get; private set; }

        public string VirtualPath
        {
            get
            {
                return UrlUtil.Combine(Template.VirtualPath, "Layouts", Name);
            }
        }

        public string AspxVirtualPath
        {
            get
            {
                return UrlUtil.Combine(VirtualPath, "Default.aspx");
            }
        }

        public string DesignerPath
        {
            get
            {
                return UrlUtil.Combine(VirtualPath, "Designer.aspx");
            }
        }

        public string PreviewImageVirtualPath
        {
            get
            {
                return UrlUtil.Combine(VirtualPath, "Preview.gif");
            }
        }

        public Template Template { get; private set; }

        private Dictionary<string, Zone> _zones = new Dictionary<string,Zone>();

        public IEnumerable<Zone> Zones
        {
            get
            {
                return _zones.Values;
            }
        }

        public Zone FindZone(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            Zone zone = null;
            if (_zones.TryGetValue(name, out zone))
            {
                return zone;
            }

            return null;
        }

        public bool ContainsZone(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            return _zones.ContainsKey(name);
        }

        public Layout(string name, Template template)
        {
            Require.NotNullOrEmpty(name, "name");
            Require.NotNull(template, "template");

            this.Name = name;
            this.Template = template;

            Configure();
        }

        private void Configure()
        {
            string path = Server.MapPath(UrlUtil.Combine(VirtualPath, "config.config"));

            if (File.Exists(path))
            {
                var xml = XDocument.Load(path).Root;
                var displayName = xml.ChildElementValue("display-name");

                if (!String.IsNullOrEmpty(displayName))
                {
                    DisplayName = new LocalizableText(displayName);
                    DisplayName.ResourceDescriptor.TemplateName = Template.Name;
                }
            }

            if (DisplayName == null)
            {
                DisplayName = new LocalizableText(String.Format("{{ Template={0}, Key={1} }}", Template.Name, Name));
            }

            var layoutParser = LayoutFileParsers.GetParser(".aspx");
            var parseResult = layoutParser.Parse(Server.MapPath(AspxVirtualPath));

            foreach (var zoneName in parseResult.ZoneNames)
            {
                _zones.Add(zoneName, new Zone(zoneName, this));
            }
        }
    }
}
