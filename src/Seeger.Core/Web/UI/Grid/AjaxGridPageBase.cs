using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Seeger.Web.UI.Grid
{
    public class AjaxGridPageBase : BackendPageBase
    {
        [WebMethod, ScriptMethod]
        public static string LoadGridHtml(AjaxGridContext context)
        {
            // This try catch is useful when debugging
            try
            {
                var controlPath = context.GetGridControlVirtualPath(HttpContext.Current.Request.FilePath);
                var control = (AjaxGridUserControlBase)ControlHelper.LoadControl(controlPath);
                control.Bind(context);

                return ControlHelper.RenderControl(control);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class AjaxGridPageBase<TSearchModel> : BackendPageBase
    {
        [WebMethod, ScriptMethod]
        public static string LoadGridHtml(AjaxGridContext<TSearchModel> context)
        {
            try
            {
                var controlPath = context.GetGridControlVirtualPath(HttpContext.Current.Request.FilePath);
                var control = (AjaxGridUserControlBase)ControlHelper.LoadControl(controlPath);
                control.Bind(context);

                return ControlHelper.RenderControl(control);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
