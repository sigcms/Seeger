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
        public abstract void Bind(AjaxGridBindingContext context);
    }

    public abstract class AjaxDataGridUserControlBase<TSearchModel> : UserControlBase
    {
        public abstract void Bind(AjaxGridBindingContext<TSearchModel> context);
    }
}
