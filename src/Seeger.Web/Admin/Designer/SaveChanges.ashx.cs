using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

using Seeger.Data;

namespace Seeger.Web.UI.Admin.Designer
{
    public class SaveChanges : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int pageId = Convert.ToInt32(context.Request.Form["pageId"]);
            string culture = context.Request.Form["culture"];
            string states = context.Request.Form["states"];

            try
            {
                var page = NhSessionManager.GetCurrentSession().Get<PageItem>(pageId);

                if (page == null)
                    throw new ArgumentException(String.Format("Page was not found. Page ID: {0}.", pageId));

                var stateItems = WidgetStateItem.Parse(states);

                var service = new DesignerLayoutService(page, CultureInfo.GetCultureInfo(culture), stateItems);
                service.SaveLayoutChanges();

                context.Response.Write(OperationResult.CreateSuccessResult().ToJson());
            }
            catch (Exception ex)
            {
                context.Response.Write(OperationResult.CreateErrorResult(ex).ToJson());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}