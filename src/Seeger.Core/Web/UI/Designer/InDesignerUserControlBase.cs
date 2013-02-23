using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Seeger.Caching;

namespace Seeger.Web.UI
{
    public abstract class InDesignerUerControlBase : UserControlBase
    {
        private PageItem _pageItem;
        public PageItem PageItem
        {
            get
            {
                if (_pageItem == null)
                {
                    _pageItem = NhSession.Get<PageItem>(PageId);
                }
                return _pageItem;
            }
        }

        private int _pageId = -1;
        public int PageId
        {
            get
            {
                if (_pageId < 0)
                {
                    Int32.TryParse(Request.QueryString["pageid"], out _pageId);

                    if (_pageId <= 0)
                        throw new InvalidOperationException("Invalid page id: " + Request.QueryString["pageid"]);
                }

                return _pageId;
            }
        }

        private CultureInfo _pageCulture;
        protected CultureInfo PageCulture
        {
            get
            {
                if (_pageCulture == null && !String.IsNullOrEmpty(Request.QueryString["page-culture"]))
                {
                    _pageCulture = CultureInfo.GetCultureInfo(Request.QueryString["page-culture"]);
                }
                return _pageCulture;
            }
        }
    }

}
