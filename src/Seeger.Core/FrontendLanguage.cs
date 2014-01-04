using Seeger.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    [Class]
    public class FrontendLanguage
    {
        private string _domain = String.Empty;

        [EntityKey]
        public virtual string Name { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string BindedDomain
        {
            get { return _domain; }
            set
            {
                value = value ?? String.Empty;
                value = value.Trim().Trim('.', '/').ToLower();

                _domain = value;
            }
        }

        public FrontendLanguage()
        {
        }
    }
}
