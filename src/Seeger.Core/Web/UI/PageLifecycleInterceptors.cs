using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.UI
{
    public static class PageLifecycleInterceptors
    {
        static readonly PageLifecycleInterceptorCollection _interceptors = new PageLifecycleInterceptorCollection();

        public static PageLifecycleInterceptorCollection Interceptors
        {
            get
            {
                return _interceptors;
            }
        }
    }
}
