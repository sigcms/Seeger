using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Seeger.Collections;
using Seeger.Globalization;

namespace Seeger.Web.UI
{
    public class XmlMenuItem : ITreeNode<XmlMenuItem>, IAdminControl
    {
        private Dictionary<string, string> _attributes;

        private LocalizableText _title;
        private LocalizableText _tooltip;

        public string Name
        {
            get { return GetAttributeValue("name"); }
            set { SetAttributeValue("name", value); }
        }

        public virtual LocalizableText Title
        {
            get
            {
                if (_title == null)
                {
                    string title = GetAttributeValue("title");
                    _title = String.IsNullOrEmpty(title) ? LocalizableText.Empty() : new LocalizableText(title);
                }
                return _title;
            }
            set
            {
                _title = value ?? LocalizableText.Empty();
            }
        }

        public virtual LocalizableText Tooltip
        {
            get
            {
                if (_tooltip == null)
                {
                    string tooltip = GetAttributeValue("tooltip");
                    _tooltip = String.IsNullOrEmpty(tooltip) ? LocalizableText.Empty() : new LocalizableText(tooltip);
                }
                return _tooltip;
            }
            set
            {
                _tooltip = value ?? LocalizableText.Empty();
            }
        }

        public virtual string NavigateUrl
        {
            get { return GetAttributeValue("url"); }
            set { SetAttributeValue("url", value); }
        }

        public virtual string Target
        {
            get { return GetAttributeValue("target"); }
            set { SetAttributeValue("target", value); }
        }

        public bool RequireSuperAdmin
        {
            get
            {
                bool require = false;
                Boolean.TryParse(GetAttributeValue("require-super-admin"), out require);

                return require;
            }
            set
            {
                SetAttributeValue("require-super-admin", value.ToString().ToLower());
            }
        }

        public string Plugin
        {
            get { return GetAttributeValue("module"); }
            set { SetAttributeValue("module", value); }
        }

        public string PermissionGroup
        {
            get { return GetAttributeValue("function"); }
            set { SetAttributeValue("function", value); }
        }

        public string Permission
        {
            get { return GetAttributeValue("operation"); }
            set { SetAttributeValue("operation", value); }
        }

        public string Feature
        {
            get { return GetAttributeValue("feature"); }
            set { SetAttributeValue("feature", value); }
        }

        public XmlMenuItem Parent { get; set; }

        public XmlMenuItemCollection Items { get; private set; }

        public XmlMenuItem()
        {
            Items = new XmlMenuItemCollection(this);
            _attributes = new Dictionary<string, string>();
        }

        public static XmlMenuItem FromXml(XElement element)
        {
            Require.NotNull(element, "element");

            XmlMenuItem menuItem = new XmlMenuItem();
            menuItem.UpdateFrom(element);

            return menuItem;
        }

        public void UpdateFrom(XElement element)
        {
            Require.NotNull(element, "element");

            Dictionary<string, string> attributes = new Dictionary<string, string>();

            foreach (var attr in element.Attributes())
            {
                attributes.Add(attr.Name.LocalName, attr.Value);
            }

            _attributes = attributes;
        }

        public string GetAttributeValue(string attributeName)
        {
            Require.NotNullOrEmpty(attributeName, "attributeName");

            string value = null;
                _attributes.TryGetValue(attributeName, out value);

            return value;
        }

        public void SetAttributeValue(string attributeName, string value)
        {
            Require.NotNullOrEmpty(attributeName, "attributeName");

            if (_attributes.ContainsKey(attributeName))
            {
                _attributes[attributeName] = value ?? String.Empty;
            }
            else
            {
                _attributes.Add(attributeName, value ?? String.Empty);
            }
        }

        IEnumerable<XmlMenuItem> ITreeNode<XmlMenuItem>.Children
        {
            get { return Items; }
        }
    }

}
