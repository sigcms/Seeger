using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Licensing
{
    class InvalidEdition : Edition
    {
        public static readonly InvalidEdition Instance = new InvalidEdition();

        public override string Name
        {
            get { return "Invalid"; }
        }

        public override string Description
        {
            get { return String.Empty; }
        }

        public override bool IsFeatureAvailable(string feature)
        {
            return false;
        }
    }
}
