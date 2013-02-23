using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Seeger.Globalization;
using System.Xml.Linq;

namespace Seeger
{
    public class TinyMceFontSettingCollection : IEnumerable<TinyMceFontSetting>
    {
        private Dictionary<string, TinyMceFontSetting> _items = new Dictionary<string,TinyMceFontSetting>(StringComparer.OrdinalIgnoreCase);
        
        public static TinyMceFontSettingCollection From(XElement xml)
        {
            var collection = new TinyMceFontSettingCollection();

            foreach (var element in xml.Elements())
            {
                var item = TinyMceFontSetting.From(element);
                collection._items.Add(item.Culture, item);
            }

            return collection;
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public TinyMceFontSetting Find(CultureInfo culture)
        {
            Require.NotNull(culture, "culture");

            return Find(culture.Name);
        }

        public TinyMceFontSetting Find(string culture)
        {
            Require.NotNullOrEmpty(culture, "culture");

            TinyMceFontSetting item = null;
            if (_items.TryGetValue(culture, out item))
            {
                return item;
            }

            return null;
        }

        public IEnumerator<TinyMceFontSetting> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
