using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Seeger.Data;
using Seeger.Plugins;
using Seeger.Plugins.Widgets;
using Seeger.Data.Mapping;
using Seeger.Data.Mapping.Attributes;

namespace Seeger
{
    [Entity, Cache]
    public class LocatedWidget
    {
        public virtual int Id { get; set; }

        public virtual int Order { get; set; }

        public virtual string PluginName { get; set; }

        public virtual string WidgetName { get; set; }
        
        public virtual string ZoneName { get; set; }

        public virtual EntityAttributeCollection Attributes { get; protected set; }

        public virtual PageItem Page { get; protected set; }

        protected LocatedWidget() { }

        public LocatedWidget(PageItem page)
        {
            Require.NotNull(page, "page");
            Page = page;
            Attributes = new EntityAttributeCollection();
        }
    }
}
