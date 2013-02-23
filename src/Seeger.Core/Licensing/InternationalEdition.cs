using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Licensing
{
    sealed class InternationalEdition : Edition
    {
        public override string Name
        {
            get { return "International"; }
        }

        public override string Description
        {
            get { return String.Empty; }
        }

        public InternationalEdition()
        {
        }

        public override bool IsFeatureAvailable(string feature)
        {
            return true;
        }
    }
}
