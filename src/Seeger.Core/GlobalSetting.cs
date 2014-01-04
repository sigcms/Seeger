using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Seeger.Data.Mapping;

namespace Seeger
{
    [Class]
    public class GlobalSetting
    {
        [EntityKey]
        public virtual string Key { get; protected set; }

        [StringClob]
        public virtual string Value { get; set; }

        protected GlobalSetting() { }

        public GlobalSetting(string key)
        {
            Require.NotNullOrEmpty(key, "key");

            this.Key = key;
        }
    }
}
