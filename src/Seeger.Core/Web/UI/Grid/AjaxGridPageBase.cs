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
        public static string LoadGridHtml(string gridId, int pageIndex, string pageRawUrl)
        {
            // This try catch is useful when debugging
            try
            {
                var bindingContext = new AjaxGridBindingContext(gridId, pageIndex, pageRawUrl);

                var controlPath = bindingContext.GetGridControlVirtualPath(HttpContext.Current.Request.FilePath);
                var control = (AjaxGridUserControlBase)ControlHelper.LoadControl(controlPath);
                control.Bind(bindingContext);

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
        public static string LoadGridHtml(string gridId, int pageIndex, string pageRawUrl, TSearchModel searchModel)
        {
            try
            {
                var bindingContext = new AjaxGridBindingContext<TSearchModel>(searchModel, gridId, pageIndex, pageRawUrl);

                var controlPath = bindingContext.GetGridControlVirtualPath(HttpContext.Current.Request.FilePath);
                var control = (AjaxGridUserControlBase)ControlHelper.LoadControl(controlPath);
                control.Bind(bindingContext);

                return ControlHelper.RenderControl(control);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
