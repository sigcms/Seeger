using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:Pager runat=server></{0}:Pager>")]
    public class Pager : WebControl
    {
        public Pager()
        {
            PageSize = 10;
            UrlFormat = "/{0}";
        }

        public int PageSize
        {
            get { return Convert.ToInt32(ViewState["PageSize"]); }
            set
            {
                if (value > 0)
                {
                    ViewState["PageSize"] = value;
                }
            }
        }

        public int RecordCount
        {
            get { return Convert.ToInt32(ViewState["RecordCount"]); }
            set { ViewState["RecordCount"] = value; }
        }

        public int PageCount
        {
            get
            {
                if (RecordCount == 0) return 0;

                int count = RecordCount / PageSize;
                if (RecordCount % PageSize > 0)
                {
                    count++;
                }

                return count;
            }
        }

        public int PageIndex
        {
            get { return Convert.ToInt32(ViewState["PageIndex"]); }
            set
            {
                if (value >= 0)
                {
                    ViewState["PageIndex"] = value;
                }
                else
                {
                    ViewState["PageIndex"] = 0;
                }
            }
        }

        public string UrlFormat
        {
            get { return (string)ViewState["UrlFormat"]; }
            set { ViewState["UrlFormat"] = value; }
        }

        public PagerUrlFieldMode UrlFieldMode
        {
            get { return ViewState.TryGetValue<PagerUrlFieldMode>("UrlFieldMode", PagerUrlFieldMode.ZeroBased); }
            set { ViewState["UrlFieldMode"] = value; }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (PageIndex >= PageCount)
            {
                PageIndex = PageCount - 1;
            }

            if (PageCount <= 1) return;

            int pageCount = PageCount;
            for (int i = 0; i < pageCount; i++)
            {
                if (PageIndex == i)
                {
                    output.Write(String.Format("<strong class='pager-current'>{0}</strong>", i + 1));
                }
                else
                {
                    output.Write(String.Format("<a class='pager-num' href='{0}'>{1}</a>", String.Format(UrlFormat, (UrlFieldMode == PagerUrlFieldMode.ZeroBased) ? i : (i + 1)), i + 1));
                }
            }
        }
    }

    public enum PagerUrlFieldMode
    {
        ZeroBased = 0,
        OneBased = 1
    }
}
