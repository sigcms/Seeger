using Seeger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> FilterWith<T>(this IQueryable<T> query, IQueryFilter<T> filter)
        {
            Require.NotNull(query, "query");
            Require.NotNull(filter, "filter");

            return filter.ApplyTo(query);
        }

        public static PagedQueryable<T> Paging<T>(this IQueryable<T> query, int pageSize)
        {
            Require.NotNull(query, "query");
            return new PagedQueryable<T>(query, pageSize);
        }
    }
}
