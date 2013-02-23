using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Templates
{
    public class Zone
    {
        public string Name { get; private set; }
        public string FullName
        {
            get { return Layout.FullName + "." + Name; }
        }
        public Layout Layout { get; private set; }

        public Zone(string name, Layout layout)
        {
            Require.NotNullOrEmpty(name, "name");
            Require.NotNull(layout, "layout");

            this.Name = name;
            this.Layout = layout;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
