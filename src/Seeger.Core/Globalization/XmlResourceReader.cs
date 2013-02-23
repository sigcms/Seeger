using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Seeger.Globalization
{
    static class XmlResourceReader
    {
        public static IDictionary<string, string> ReadFrom(string xmlFilePath)
        {
            var xml = XDocument.Load(xmlFilePath).Root;
            var prefix = xml.AttributeValue("prefix");

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.EndsWith(".") ? prefix : prefix + ".";
            }

            var items = new Dictionary<string, string>();

            foreach (var element in xml.Elements())
            {
                var key = element.AttributeOrChildElementValue("key");

                if (String.IsNullOrEmpty(key))
                    throw new InvalidOperationException("Missing 'key' attribute or child element in localization file: " + xmlFilePath);

                key = prefix + key;

                if (items.ContainsKey(key))
                    throw new InvalidOperationException("Key '" + key + "' already exists.");

                var value = element.AttributeOrChildElementValue("value");
                if (String.IsNullOrEmpty(value) && !element.HasElements)
                {
                    value = element.Value;
                }

                items.Add(key, value);
            }

            return items;
        }
    }
}
