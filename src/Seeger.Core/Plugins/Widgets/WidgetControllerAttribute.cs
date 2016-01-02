using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seeger.Plugins.Widgets
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WidgetControllerAttribute : Attribute
    {
        public string WidgetName { get; set; }

        public WidgetControllerAttribute() { }

        public WidgetControllerAttribute(string widgetName)
        {
            WidgetName = widgetName;
        }
    }
}
