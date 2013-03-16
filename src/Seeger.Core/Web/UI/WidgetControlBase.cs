using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Caching;
using Seeger.Data;
using Seeger.Plugins.Widgets;
using Seeger.Plugins;

namespace Seeger.Web.UI
{
    public abstract class WidgetControlBase : UserControlBase
    {
        public new LayoutPageBase Page
        {
            get
            {
                LayoutPageBase templatePage = base.Page as LayoutPageBase;
                if (templatePage == null)
                {
                    throw new InvalidOperationException(
                        String.Format("Widget can only be placed in template page, which is of type {0}.", typeof(LayoutPageBase).FullName));
                }
                return templatePage;
            }
        }

        public LocatedWidget WidgetInPage { get; internal set; }

        public WidgetDefinition Widget
        {
            get
            {
                var plugin = PluginManager.FindEnabledPlugin(WidgetInPage.PluginName);
                if (plugin != null)
                {
                    return plugin.FindWidget(WidgetInPage.WidgetName);
                }

                return null;
            }
        }

        public EntityAttributeCollection WidgetAttributes
        {
            get
            {
                if (WidgetInPage == null)
                {
                    throw new InvalidOperationException("WidgetInPage is required.");
                }
                return WidgetInPage.Attributes;
            }
        }

        public int PageId
        {
            get { return Page.PageId; }
        }

        public string Suffix
        {
            get { return Page.Suffix; }
        }

        public PageItem PageItem
        {
            get { return Page.PageItem; }
        }

        public SEOInfo Seo
        {
            get { return Page.SEOInfo; }
        }
    }
}
