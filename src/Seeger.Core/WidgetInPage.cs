using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Seeger.Data;
using Seeger.Plugins;
using Seeger.Plugins.Widgets;

namespace Seeger
{
    public class WidgetInPage
    {
        public virtual int Id { get; set; }
        public virtual int Order { get; set; }

        public virtual string PluginName { get; set; }

        public virtual string WidgetName { get; set; }
        
        public virtual string ZoneName { get; set; }

        public virtual PageItem Page { get; protected set; }

        protected WidgetInPage() { }

        public WidgetInPage(PageItem page)
        {
            Require.NotNull(page, "page");

            Page = page;
            PluginName = String.Empty;
        }

        private EntityAttributeCollection _attributes = null;
        public virtual EntityAttributeCollection Attributes
        {
            get
            {
                if (_attributes == null)
                {
                    _attributes = new EntityAttributeCollection();
                }
                return _attributes;
            }
            protected set
            {
                _attributes = value;
            }
        }
    }

}
