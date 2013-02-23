using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Seeger;

namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static void EnsureItemAdded<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue valueIfNotYetAdded)
        {
            Require.NotNull(dictionary, "dictionary");
            Require.NotNull(key, "key");

            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, valueIfNotYetAdded);
            }
        }
    }
}
