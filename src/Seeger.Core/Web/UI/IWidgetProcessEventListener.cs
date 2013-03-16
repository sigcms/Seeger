using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Data;
using System.Globalization;

namespace Seeger.Web.UI
{
    public interface IWidgetProcessEventListener
    {
        void OnProcessing(WidgetProcessEventArgs e);
        void OnProcessed(WidgetProcessEventArgs e);
    }

    public class WidgetProcessEventArgs : EventArgs
    {
        public PageItem CurrentPage { get; internal set; }
        public CultureInfo DesignerCulture { get; internal set; }
        public WidgetStateItem StateItem { get; internal set; }
        public LocatedWidget WidgetInPage { get; internal set; }

        public WidgetProcessEventArgs()
        {
        }
    }
}
