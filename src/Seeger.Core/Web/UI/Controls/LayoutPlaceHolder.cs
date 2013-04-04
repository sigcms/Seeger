using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI
{
    public class LayoutPlaceHolder : Control
    {
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }

        public LayoutPlaceHolderVisibleWhen VisibleWhen
        {
            get
            {
                return ViewState.TryGetValue<LayoutPlaceHolderVisibleWhen>("VisibleWhen", LayoutPlaceHolderVisibleWhen.InLiveMode);
            }
            set
            {
                ViewState["VisibleWhen"] = value;
            }
        }

        protected bool IsInDesignMode
        {
            get
            {
                var page = Page as LayoutPageBase;
                return page != null && page.IsInDesignMode;
            }
        }

        protected override void RenderChildren(HtmlTextWriter writer)
        {
            if (VisibleWhen == LayoutPlaceHolderVisibleWhen.InLiveMode && IsInDesignMode)
            {
                return;
            }
            if (VisibleWhen == LayoutPlaceHolderVisibleWhen.InDesignMode && !IsInDesignMode)
            {
                return;
            }

            base.RenderChildren(writer);
        }
    }

    public enum LayoutPlaceHolderVisibleWhen
    {
        InLiveMode = 0,
        InDesignMode = 1
    }
}
