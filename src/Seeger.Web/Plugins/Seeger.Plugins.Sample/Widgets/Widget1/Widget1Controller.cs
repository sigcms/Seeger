using Seeger.Plugins.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Sample.Widgets.Widget1
{
    [WidgetController]
    public class Widget1Controller : DefaultWidgetController
    {
        public override void PreRender(WidgetContext context)
        {
            context.Model = new Article
            {
                Title = "Introducing the widget view feature",
                Content = "Do you like it?"
            };
        }
    }

    public class Article
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}