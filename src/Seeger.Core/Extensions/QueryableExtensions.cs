using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    public static class QueryableExtensions
    {
        public static PagedQueryable<T> Paging<T>(this IQueryable<T> query, int pageSize)
        {
            return new PagedQueryable<T>(query, pageSize);
        }
    }
}
