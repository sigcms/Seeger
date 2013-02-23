using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Collections;

namespace Seeger.Data
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> FilterWith<T>(this IQueryable<T> src, IQueryFilter<T> filter)
        {
            Require.NotNull(src, "src");
            Require.NotNull(filter, "filter");

            return filter.ApplyTo(src);
        }

        public static PagedList<T> Paging<T>(this IQueryable<T> src, int pageSize)
        {
            Require.NotNull(src, "src");

            return new PagedList<T>(src, pageSize);
        }
    }
}
