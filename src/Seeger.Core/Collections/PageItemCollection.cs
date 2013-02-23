using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    public class PageItemCollection : IEnumerable<PageItem>
    {
        private PageItem _parent;
        private LinkedList<PageItem> _innerList = new LinkedList<PageItem>();

        public PageItemCollection(PageItem parent)
            : this(parent, Enumerable.Empty<PageItem>())
        {
        }

        public PageItemCollection(PageItem parent, IEnumerable<PageItem> items)
        {
            _parent = parent;
            foreach (var item in items.OrderBy(p => p.Order))
            {
                _innerList.AddLast(item);
            }
        }

        public int Count
        {
            get { return _innerList.Count; }
        }

        public PageItem First
        {
            get
            {
                if (_innerList.First != null)
                {
                    return _innerList.First.Value;
                }
                return null;
            }
        }

        public PageItem Last
        {
            get
            {
                if (_innerList.Last != null)
                {
                    return _innerList.Last.Value;
                }
                return null;
            }
        }

        public bool Contains(int pageId)
        {
            return _innerList.Any(it => it.Id == pageId);
        }

        public bool Contains(PageItem page)
        {
            return _innerList.Contains(page);
        }

        public void AddFirst(PageItem item)
        {
            Require.NotNull(item, "item");

            item.Parent = _parent;

            if (Count > 0)
            {
                item.Order = First.Order - 5;
                if (item.Order < 0)
                {
                    item.Order = 0;
                    RecitifyOrderProperties(_innerList.First);
                }
            }

            _innerList.AddFirst(item);
        }

        public void AddBefore(PageItem item, int refItemId)
        {
            PageItem refItem = _innerList.FirstOrDefault(p => p.Id == refItemId);
            if (refItem == null)
                throw new InvalidOperationException("Refrenced page was not found. Refrenced page id: " + refItemId);

            AddBefore(item, _innerList.First(p => p.Id == refItemId));
        }

        public void AddBefore(PageItem item, PageItem refItem)
        {
            var node = _innerList.Find(item);
            if (node == null)
            {
                node = new LinkedListNode<PageItem>(item);
            }

            AddBefore(node, _innerList.Find(refItem));
        }

        private void AddBefore(LinkedListNode<PageItem> node, LinkedListNode<PageItem> refNode)
        {
            Require.NotNull(node, "node");
            Require.NotNull(refNode, "refNode");

            if (node.List != _innerList)
            {
                AddPageItem(node.Value);
            }

            _innerList.AddBefore(refNode, node);

            node.Value.Order = refNode.Value.Order - 5;
            if (node.Value.Order < 0)
            {
                node.Value.Order = 0;
            }

            if (node.Previous != null)
            {
                RecitifyOrderProperties(node.Previous);
            }
            else
            {
                RecitifyOrderProperties(node);
            }
        }

        public void AddAfter(PageItem item, int refItemId)
        {
            PageItem refItem = _innerList.FirstOrDefault(p => p.Id == refItemId);
            if (refItem == null)
            {
                throw new InvalidOperationException("Refrenced page was not found. Refrenced page id: " + refItemId);
            }

            AddAfter(item, _innerList.First(p => p.Id == refItemId));
        }

        public void AddAfter(PageItem item, PageItem refItem)
        {
            var node = _innerList.Find(item);
            if (node == null)
            {
                node = new LinkedListNode<PageItem>(item);
            }

            AddAfter(node, _innerList.Find(refItem));
        }

        private void AddAfter(LinkedListNode<PageItem> node, LinkedListNode<PageItem> refNode)
        {
            Require.NotNull(node, "node");
            Require.NotNull(refNode, "refNode");

            AddPageItem(node.Value);

            _innerList.AddAfter(refNode, node);

            node.Value.Order = refNode.Value.Order + 5;
            RecitifyOrderProperties(node);
        }

        public void AddLast(PageItem item)
        {
            Require.NotNull(item, "item");

            AddPageItem(item);

            item.Order = 0;
            if (Count > 0)
            {
                item.Order = Last.Order + 5;
            }
        }

        public void MoveBefore(PageItem item, PageItem refItem)
        {
            var node = _innerList.Find(item);
            var refNode = _innerList.Find(refItem);

            if (node == null || refNode == null)
                throw new InvalidOperationException("The two items must be in the list.");

            _innerList.Remove(node);
            node = _innerList.AddBefore(refNode, item);

            if (node.Previous == null) // is first
            {
                node.Value.Order = refItem.Order - 5;
                RecitifyOrderProperties(node);
            }
            else
            {
                node.Value.Order = (node.Previous.Value.Order + node.Next.Value.Order) / 2;
                if (node.Value.Order == node.Previous.Value.Order)
                {
                    RecitifyOrderProperties(node.Previous);
                }
            }
        }

        public void MoveAfter(PageItem item, PageItem refItem)
        {
            var node = _innerList.Find(item);
            var refNode = _innerList.Find(refItem);

            if (node == null || refNode == null)
                throw new InvalidOperationException("The two items must be in the same list");

            _innerList.Remove(node);
            node = _innerList.AddAfter(refNode, item);

            if (node.Next == null)
            {
                node.Value.Order = refItem.Order + 5;
            }
            else
            {
                node.Value.Order = (refItem.Order + node.Next.Value.Order) / 2;
                if (node.Value.Order == refItem.Order)
                {
                    RecitifyOrderProperties(node.Previous);
                }
            }
        }

        public void Remove(PageItem item)
        {
            Require.NotNull(item, "item");

            _innerList.Remove(item);
        }

        private void AddPageItem(PageItem item)
        {
            if (item.Parent == _parent) return;

            if (item.Parent != null)
            {
                item.Parent.Pages.Remove(item);
            }

            item.Parent = _parent;
        }

        private void RecitifyOrderProperties(LinkedListNode<PageItem> start)
        {
            if (start.Value.Order < 0)
            {
                start.Value.Order = 0;
            }

            LinkedListNode<PageItem> current = start;
            LinkedListNode<PageItem> next = current.Next;

            while (next != null)
            {
                if (next.Value.Order <= current.Value.Order)
                {
                    next.Value.Order = current.Value.Order + 5;
                }

                current = next;
                next = current.Next;
            }
        }

        public IEnumerator<PageItem> GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
