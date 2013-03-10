using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Collections
{
    public class PageItemCollectionWrapper : IEnumerable<PageItem>
    {
        private List<PageItem> _pages;

        public int Count
        {
            get
            {
                return _pages.Count;
            }
        }

        public PageItemCollectionWrapper(IEnumerable<PageItem> pages)
        {
            _pages = new List<PageItem>(pages);
        }

        public PageItem FindById(int pageId)
        {
            return _pages.FirstOrDefault(p => p.Id == pageId);
        }

        public void AddFirst(PageItem page)
        {
            _pages.Insert(0, page);
        }

        public void AddLast(PageItem page)
        {
            _pages.Add(page);
        }

        public void AddBefore(PageItem page, PageItem pageToAdd)
        {
            var index = ValidateAndGetIndex(page);
            _pages.Insert(index, pageToAdd);
        }

        public void AddAfter(PageItem page, PageItem pageToAdd)
        {
            var index = ValidateAndGetIndex(page);
            
            // if page is the last page
            if (index == _pages.Count - 1)
            {
                _pages.Add(pageToAdd);
            }
            else
            {
                _pages.Insert(index + 1, pageToAdd);
            }
        }

        public void MoveBefore(PageItem page, PageItem pageToMove)
        {
            ValidateAndGetIndex(pageToMove);
            _pages.Remove(pageToMove);
            AddBefore(page, pageToMove);
        }

        public void MoveAfter(PageItem page, PageItem pageToMove)
        {
            ValidateAndGetIndex(pageToMove);
            _pages.Remove(pageToMove);
            AddAfter(page, pageToMove);
        }

        private int ValidateAndGetIndex(PageItem page)
        {
            var index = -1;

            for (var i = 0; i < _pages.Count; i++)
            {
                if (_pages[i].Id == page.Id)
                {
                    index = i; break;
                }
            }

            if (index < 0)
                throw new InvalidOperationException("The specified page was not found in the collection.");

            return index;
        }

        public IEnumerator<PageItem> GetEnumerator()
        {
            return _pages.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
