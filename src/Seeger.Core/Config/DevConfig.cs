using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Seeger.Config
{
    public class DevConfig
    {
        public bool ShowDevPanels { get; set; }

        public static DevConfig From(XElement xml)
        {
            return new DevConfig
            {
                ShowDevPanels = xml.ChildElementValue<bool>("show-dev-panels")
            };
        }
    }
}
