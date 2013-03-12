using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.UI.Grid
{
    public abstract class AjaxGridUserControlBase : UserControlBase
    {
        public abstract void Bind(AjaxGridContext context);
    }

    public abstract class AjaxGridUserControlBase<TSearchModel> : UserControlBase
    {
        public abstract void Bind(AjaxGridContext<TSearchModel> context);
    }
}
