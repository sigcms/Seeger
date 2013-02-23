using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Plugins.Widgets.Loaders
{
    public interface IWidgetLoader
    {
        WidgetDefinition LoadWidget(PluginDefinition plugin, string widgetName);
    }
}
