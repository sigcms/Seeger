using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Globalization;
using System.Web.Compilation;
using System.Web;

using Seeger.Web;

namespace Seeger.Globalization
{
    class XmlResourceProvider : IResourceProvider
    {
        public object GetObject(string resourceDescriptor, System.Globalization.CultureInfo culture)
        {
            var descriptor = ResourceDescriptor.Parse(resourceDescriptor);

            var localizer = SmartLocalizer.GetForCurrentRequest();

            if (String.IsNullOrEmpty(descriptor.PluginName) && !String.IsNullOrEmpty(localizer.PluginName))
            {
                descriptor.PluginName = localizer.PluginName;
            }
            if (String.IsNullOrEmpty(descriptor.TemplateName) && !String.IsNullOrEmpty(localizer.TemplateName))
            {
                descriptor.TemplateName = localizer.TemplateName;
            }
            if (String.IsNullOrEmpty(descriptor.WidgetName) && !String.IsNullOrEmpty(localizer.WidgetName))
            {
                descriptor.WidgetName = localizer.WidgetName;
            }

            return descriptor.Localize(culture ?? CultureInfo.CurrentUICulture);
        }

        public IResourceReader ResourceReader
        {
            get { throw new NotSupportedException(); }
        }
    }

}
