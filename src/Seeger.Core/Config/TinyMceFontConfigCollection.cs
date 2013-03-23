using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Seeger.Globalization;
using System.Xml.Linq;

namespace Seeger.Config
{
    public class TinyMceFontConfigCollection : IEnumerable<TinyMceFontConfig>
    {
        private Dictionary<string, TinyMceFontConfig> _items = new Dictionary<string,TinyMceFontConfig>(StringComparer.OrdinalIgnoreCase);
        
        public static TinyMceFontConfigCollection From(XElement xml)
        {
            var collection = new TinyMceFontConfigCollection();

            foreach (var element in xml.Elements())
            {
                var item = TinyMceFontConfig.From(element);
                collection._items.Add(item.Culture, item);
            }

            return collection;
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public TinyMceFontConfig Find(CultureInfo culture)
        {
            Require.NotNull(culture, "culture");

            return Find(culture.Name);
        }

        public TinyMceFontConfig Find(string culture)
        {
            Require.NotNullOrEmpty(culture, "culture");

            TinyMceFontConfig item = null;
            if (_items.TryGetValue(culture, out item))
            {
                return item;
            }

            return null;
        }

        public IEnumerator<TinyMceFontConfig> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
