using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Seeger.Web;
using Seeger.Templates;

namespace Seeger.Templates
{
    public class TemplateSkin : Skin
    {
        public Template Template { get; private set; }

        public string FullName
        {
            get
            {
                return Template.Name + "." + Name;
            }
        }

        public TemplateSkin(string name, Template template)
            : this(name, template, String.Format("{{ Template={1}, Key={2} }}", template.Name, template.Name, name))
        {
        }

        public TemplateSkin(string name, Template template, string displayName)
            : base(name, UrlUtil.Combine(template.SkinFolderVirtualPath, name), displayName)
        {
            Template = template;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
