using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Comments.Api
{
    public class PagedDataList<T>
    {
        public List<T> Items { get; set; }

        public int TotalItems { get; set; }
    }
}