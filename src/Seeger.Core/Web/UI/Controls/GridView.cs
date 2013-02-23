using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;

namespace Seeger.Web.UI
{
    public delegate void NeedDataSourceEventHandler(object sender, EventArgs e);

    public class GridView : System.Web.UI.WebControls.GridView
    {
        private Pager _pager;

        public GridView()
        {
            AllowPaging = true;
        }

        public event NeedDataSourceEventHandler NeedDataSource
        {
            add
            {
                Events.AddHandler("NeedDataSource", value);
            }
            remove
            {
                Events.RemoveHandler("NeedDataSource", value);
            }
        }

        public int VirtualItemsCount
        {
            get { return ViewState.TryGetValue<int>("VirtualItemsCount", 0); }
            set
            {
                ViewState["VirtualItemsCount"] = value;

                EnsureChildControls();
                _pager.RecordCount = value;
            }
        }

        public string PagerCssClass
        {
            get
            {
                EnsureChildControls();
                return _pager.CssClass;
            }
            set
            {
                EnsureChildControls();
                _pager.CssClass = value;
            }
        }

        public string PagerUrlFormat
        {
            get
            {
                EnsureChildControls();
                return _pager.UrlFormat;
            }
            set
            {
                EnsureChildControls();
                _pager.UrlFormat = value;
            }
        }

        protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
        {
        }

        public override int PageIndex
        {
            get
            {
                return base.PageIndex;
            }
            set
            {
                base.PageIndex = value;

                EnsureChildControls();
                _pager.PageIndex = value;
            }
        }

        public override int PageSize
        {
            get
            {
                return base.PageSize;
            }
            set
            {
                base.PageSize = value;

                EnsureChildControls();
                _pager.PageSize = value;
            }
        }

        protected override void CreateChildControls()
        {
            _pager = new Pager();

            base.CreateChildControls();
        }

        protected override void OnDataBinding(EventArgs e)
        {
            OnNeedDataSource();
            base.OnDataBinding(e);
        }

        protected virtual void OnNeedDataSource()
        {
            NeedDataSourceEventHandler handler = Events["NeedDataSource"] as NeedDataSourceEventHandler;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected override void RenderChildren(System.Web.UI.HtmlTextWriter writer)
        {
            base.RenderChildren(writer);

            if (AllowPaging)
            {
                _pager.RenderControl(writer);
            }
        }
    }
}
