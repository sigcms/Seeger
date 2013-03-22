using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Linq;
using System.Globalization;

using Seeger.Web;
using Seeger.Globalization;
using System.Web.Hosting;

namespace Seeger.Templates
{
    public class Template
    {
        public LayoutCollection Layouts { get; private set; }

        public Layout FindLayout(string name)
        {
            return Layouts.FindLayout(name);
        }

        public TemplateSkinCollection Skins { get; private set; }

        public TemplateSkin FindSkin(string name)
        {
            return Skins.Find(name);
        }

        public string Localize(string key, CultureInfo culture)
        {
            return Localize(key, culture, true);
        }

        public string Localize(string key, CultureInfo culture, bool searchUp)
        {
            var value = ResourcesFolder.GetValue(key, culture);
            if (value == null && searchUp)
            {
                value = ResourceFolder.Global.GetValue(key, culture);
            }

            return value ?? key;
        }

        public string Name { get; private set; }

        public LocalizableText DisplayName { get; private set; }

        public string VirtualPath
        {
            get
            {
                return "/Templates/" + Name;
            }
        }

        public string ResourceFolderVirtualPath
        {
            get
            {
                return UrlUtil.Combine(VirtualPath, "Resources");
            }
        }

        public string SkinFolderVirtualPath
        {
            get
            {
                return UrlUtil.Combine(VirtualPath, "Skins");
            }
        }

        public ResourceFolder ResourcesFolder { get; private set; }

        public Template(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            Name = name;
            DisplayName = new LocalizableText(String.Format("{{ Template={0}, Key={1} }}", name, name));
            Skins = new TemplateSkinCollection(this);
            Layouts = new LayoutCollection(this);
            ResourcesFolder = new ResourceFolder(HostingEnvironment.MapPath(ResourceFolderVirtualPath));
        }

        public void SetDisplayName(LocalizableText displayName)
        {
            DisplayName = displayName;
            DisplayName.ResourceDescriptor.TemplateName = Name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
