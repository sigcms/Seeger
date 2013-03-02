using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    public class PagedQueryable<T>
    {
        private int _count = -1;
        private IQueryable<T> _rawQuery;

        public int Count
        {
            get
            {
                if (_count < 0)
                {
                    _count = _rawQuery.Count();
                }

                return _count;
            }
        }

        public int PageSize { get; private set; }

        public PagedQueryable(IQueryable<T> query, int pageSize)
        {
            Require.NotNull(query, "query");
            _rawQuery = query;
            PageSize = pageSize;
        }

        public IQueryable<T> Page(int pageIndex)
        {
            return _rawQuery.Skip(pageIndex * PageSize).Take(PageSize);
        }
    }
}
