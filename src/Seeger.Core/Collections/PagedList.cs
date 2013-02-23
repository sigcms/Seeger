using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Collections
{
    public class PagedList<T>
    {
        private IQueryable<T> _source;
        private int _count = -1;

        public int Count
        {
            get
            {
                if (_count == -1)
                {
                    _count = _source.Count();
                }
                return _count;
            }
        }

        public int PageSize { get; private set; }

        public PagedList(IQueryable<T> source, int pageSize)
        {
            Require.NotNull(source, "source");
            Require.That(pageSize > 0, "'pageSize' should be greater than 0.");

            _source = source;
            PageSize = pageSize;
        }

        public IQueryable<T> GetPage(int pageIndex)
        {
            Require.That(pageIndex >= 0, "'pageIndex' should not be less than zero.");

            return _source.Skip(PageSize * pageIndex).Take(PageSize);
        }
    }
}
