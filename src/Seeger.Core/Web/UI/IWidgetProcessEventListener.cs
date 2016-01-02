using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Data;
using System.Globalization;

namespace Seeger.Web.UI
{
    // TODO: Move to IWidgetController
    public interface IWidgetProcessEventListener
    {
        void OnProcessing(WidgetProcessEventArgs e);
        void OnProcessed(WidgetProcessEventArgs e);
    }

    public class WidgetProcessEventArgs : EventArgs
    {
        public PageItem CurrentPage { get; internal set; }
        public CultureInfo DesignerCulture { get; internal set; }
        public LocatedWidgetViewModel LocatedWidgetViewModel { get; internal set; }
        public LocatedWidget LocatedWidget { get; internal set; }

        public WidgetProcessEventArgs()
        {
        }
    }
}
