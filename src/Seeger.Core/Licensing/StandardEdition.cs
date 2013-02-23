using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Licensing
{
    sealed class StandardEdition : Edition
    {
        private HashSet<string> _unavailableFeatures;

        public StandardEdition()
        {
            _unavailableFeatures = new HashSet<string>();
            _unavailableFeatures.Add(Features.Multilingual);
        }

        public override string Name
        {
            get { return "Standard"; }
        }

        public override string Description
        {
            get { return String.Empty; }
        }

        public override bool IsFeatureAvailable(string feature)
        {
            return !_unavailableFeatures.Contains(feature);
        }
    }
}
