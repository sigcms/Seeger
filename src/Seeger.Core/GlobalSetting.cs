using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Seeger
{
    public class GlobalSetting
    {
        [EntityKey]
        public virtual string Key { get; protected set; }
        public virtual string Value { get; set; }

        protected GlobalSetting() { }

        public GlobalSetting(string key)
        {
            Require.NotNullOrEmpty(key, "key");

            this.Key = key;
        }
    }
}
