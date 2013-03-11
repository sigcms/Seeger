using Seeger.Globalization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
            UrlFormat = "/{0}";
        }

        public int PageSize
        {
            get
            {
                return ViewState.TryGetValue<int>("PageSize", 15);
            }
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

        public int PageButtonCount
        {
            get
            {
                return ViewState.TryGetValue<int>("PageButtonCount", 11);
            }
            set
            {
                ViewState["PageButtonCount"] = value;
            }
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

        public bool ShowPrevNextButtons
        {
            get
            {
                return ViewState.TryGetValue<bool>("ShowPrevNextButtons", true);
            }
            set
            {
                ViewState["ShowPrevNextButtons"] = value;
            }
        }

        public string PrevButtonText
        {
            get
            {
                var text = ViewState["PrevButtonText"] as string;
                if (String.IsNullOrEmpty(text))
                {
                    text = ResourcesFolder.Global.GetValue("Common.PrevPage", CultureInfo.CurrentUICulture);
                }
                return text;
            }
            set
            {
                ViewState["PrevButtonText"] = value;
            }
        }

        public string NextButtonText
        {
            get
            {
                var text = ViewState["NextButtonText"] as string;
                if (String.IsNullOrEmpty(text))
                {
                    text = ResourcesFolder.Global.GetValue("Common.NextPage", CultureInfo.CurrentUICulture);
                }
                return text;
            }
            set
            {
                ViewState["NextButtonText"] = value;
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

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (PageIndex >= PageCount)
            {
                PageIndex = PageCount - 1;
            }

            if (PageCount <= 1) return;

            var pageCount = PageCount;
            var pageIndex = PageIndex;

            if (pageCount > 1 && pageIndex > 0 && ShowPrevNextButtons)
            {
                writer.Write(String.Format("<a class=\"pager-prev\" href=\"{0}\" data-page=\"{1}\">{2}</a>", GetPageUrl(pageIndex - 1), pageIndex - 1, PrevButtonText));
            }

            if (pageCount <= PageButtonCount)
            {
                for (var i = 0; i < pageCount; i++)
                {
                    RenderPageButton(i, PageIndex, writer);
                }
            }
            else
            {
                RenderPageButton(0, pageIndex, writer);

                var buttonCount = PageButtonCount - 2 - 1; // 2: first and last page, 1: current page
                var startPageIndex = pageIndex - buttonCount / 2;
                var endPageIndex = pageIndex + buttonCount / 2 + buttonCount % 2;

                if (startPageIndex < 1)
                {
                    endPageIndex += Math.Abs(startPageIndex - 1);
                    startPageIndex = 1;
                }
                if (endPageIndex > pageCount - 2)
                {
                    startPageIndex -= endPageIndex - (pageCount - 2);
                    endPageIndex = pageCount - 2;

                    if (startPageIndex < 1)
                    {
                        startPageIndex = 1;
                    }
                }

                if (startPageIndex > 1)
                {
                    writer.Write("<span class=\"pager-dots\">...</span>");
                }

                for (var i = startPageIndex; i <= endPageIndex; i++)
                {
                    RenderPageButton(i, pageIndex, writer);
                }

                if (endPageIndex < pageCount - 2)
                {
                    writer.Write("<span class=\"pager-dots\">...</span>");
                }

                RenderPageButton(pageCount - 1, pageIndex, writer);
            }

            if (pageCount > 1 && pageIndex < pageCount - 1 && ShowPrevNextButtons)
            {
                writer.Write(String.Format("<a class=\"pager-next\" href=\"{0}\" data-page=\"{1}\">{2}</a>", GetPageUrl(pageIndex + 1), pageIndex + 1, NextButtonText));
            }
        }

        private void RenderPageButton(int pageIndex, int currentPageIndex, HtmlTextWriter writer)
        {
            if (pageIndex == currentPageIndex)
            {
                writer.Write(String.Format("<span class=\"pager-current\">{0}</span>", pageIndex + 1));
            }
            else
            {
                writer.Write(String.Format("<a class=\"pager-num\" href=\"{0}\" data-page=\"{1}\">{2}</a>", GetPageUrl(pageIndex), pageIndex, pageIndex + 1));
            }
        }

        private string GetPageUrl(int pageIndex)
        {
            return String.Format(UrlFormat, (UrlFieldMode == PagerUrlFieldMode.ZeroBased) ? pageIndex : pageIndex + 1);
        }
    }

    public enum PagerUrlFieldMode
    {
        ZeroBased = 0,
        OneBased = 1
    }
}
