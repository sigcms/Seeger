using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger;

namespace System.Xml.Linq
{
    public static class XElementExtensions
    {
        public static string ChildElementValue(this XElement element, string childElementName, string defaultValue = null)
        {
            Require.NotNull(element, "element");
            Require.NotNullOrEmpty(childElementName, "childElementName");

            var child = element.Element(childElementName);
            return child == null ? defaultValue : child.Value;
        }

        public static T ChildElementValue<T>(this XElement element, string childElementName, T defaultValue = default(T))
        {
            Require.NotNull(element, "element");
            Require.NotNullOrEmpty(childElementName, "childElementName");

            var value = ChildElementValue(element, childElementName);
            return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
        }

        public static string AttributeValue(this XElement element, string attrName, string defaultValue = null)
        {
            Require.NotNull(element, "element");
            Require.NotNullOrEmpty(attrName, "attrName");

            var attr = element.Attribute(attrName);
            return attr == null ? defaultValue : attr.Value;
        }

        public static T AttributeValue<T>(this XElement element, string attrName, T defaultValue = default(T))
        {
            Require.NotNull(element, "element");
            Require.NotNullOrEmpty(attrName, "attrName");

            var value = AttributeValue(element, attrName);
            return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
        }

        public static string AttributeOrChildElementValue(this XElement element, string attrOrElementName, string defaultValue = null)
        {
            var attrValue = AttributeValue(element, attrOrElementName);
            if (attrValue == null)
            {
                attrValue = ChildElementValue(element, attrOrElementName);
            }

            if (attrValue == null)
            {
                attrValue = defaultValue;
            }

            return attrValue;
        }

        public static T AttributeOrChildElementValue<T>(this XElement element, string attrOrElementName, T defaultValue = default(T))
        {
            var value = AttributeOrChildElementValue(element, attrOrElementName);
            return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
