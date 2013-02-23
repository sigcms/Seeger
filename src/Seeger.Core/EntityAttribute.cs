using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    public class EntityAttribute
    {
        public string Key { get; private set; }

        public string Value { get; set; }

        public EntityAttribute(string key)
        {
            Require.NotNullOrEmpty(key, "key");
            Key = key;
        }

        public EntityAttribute(string key, string value)
        {
            Require.NotNullOrEmpty(key, "key");
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}", Key, Value == null ? "<NULL>" : Value);
        }
    }
}
