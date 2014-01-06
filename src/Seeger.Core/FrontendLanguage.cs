using Seeger.Data.Mapping;
using Seeger.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    [Entity, Cache]
    public class FrontendLanguage
    {
        private string _domain = String.Empty;

        [Id]
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
