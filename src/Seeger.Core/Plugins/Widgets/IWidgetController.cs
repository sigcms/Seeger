using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seeger.Plugins.Widgets
{
    public interface IWidgetController
    {
        void PreRender(WidgetContext context);
    }

    public class DefaultWidgetController : IWidgetController
    {
        public virtual void PreRender(WidgetContext context)
        {
        }
    }

    public class WidgetContext
    {
        public PageItem Page { get; set; }

        public LocatedWidget LocatedWidget { get; set; }

        public object Model { get; set; }
    }
}
