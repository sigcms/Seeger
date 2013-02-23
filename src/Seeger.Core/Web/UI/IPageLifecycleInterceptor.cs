using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Seeger.Web.UI
{
    public interface IPageLifecycleInterceptor
    {
        void OnInit(Page page);

        void OnLoad(Page page);

        void OnPreRender(Page page);

        void OnUnload(Page page);
    }

    public class EmptyPageLifecycleInterceptor : IPageLifecycleInterceptor
    {
        public virtual void OnInit(Page page) { }

        public virtual void OnLoad(Page page) { }

        public virtual void OnPreRender(Page page) { }

        public virtual void OnUnload(Page page) { }
    }
}
