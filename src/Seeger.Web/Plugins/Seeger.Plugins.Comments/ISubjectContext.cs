using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Seeger.Plugins.Comments
{
    public interface ISubjectContext
    {
        Subject GetCurrentSubject(PageItem page, HttpContextBase httpContext);
    }

    public static class SubjectContexts
    {
        public static ISubjectContext Current = new DefaultSubjectContext();
    }
}
