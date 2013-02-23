using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public class XmlMenuItemCollection : IEnumerable<XmlMenuItem>
    {
        private XmlMenuItem _parent;
        private List<XmlMenuItem> _items;

        public XmlMenuItemCollection()
            : this(null)
        {
        }

        public XmlMenuItemCollection(XmlMenuItem parent)
        {
            _parent = parent;
            _items = new List<XmlMenuItem>();
        }

        public XmlMenuItem this[int index]
        {
            get
            {
                return _items[index];
            }
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public void Add(XmlMenuItem item)
        {
            Require.NotNull(item, "item");

            item.Parent = _parent;
            _items.Add(item);
        }

        public void Remove(XmlMenuItem item)
        {
            Require.NotNull(item, "item");

            _items.Remove(item);
        }

        public IEnumerator<XmlMenuItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
