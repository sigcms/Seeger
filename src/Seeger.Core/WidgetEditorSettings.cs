using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Seeger
{
    public class WidgetEditorSettings
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool AutoOpen { get; set; }

        public static WidgetEditorSettings From(XElement xml)
        {
            return new WidgetEditorSettings
            {
                Width = xml.ChildElementValue<int>("width"),
                Height = xml.ChildElementValue<int>("height"),
                AutoOpen = xml.ChildElementValue<bool>("auto-open")
            };
        }
    }
}
