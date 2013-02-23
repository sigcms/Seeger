using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Licensing
{
    public abstract class Edition
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract bool IsFeatureAvailable(string feature);

        internal Edition() { }

        public override string ToString()
        {
            return Name;
        }
    }
}
