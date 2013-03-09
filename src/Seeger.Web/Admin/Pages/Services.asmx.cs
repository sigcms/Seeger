using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using Seeger.Data;
using Seeger.Collections;
using Seeger.Caching;
using Seeger.Services;

namespace Seeger.Web.UI.Admin.Pages
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class Services : System.Web.Services.WebService
    {
        [WebMethod]
        public OperationResult CascadeDelete(int pageId)
        {
            try
            {
                PageService.Delete(pageId, true);

                return OperationResult.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                return OperationResult.CreateErrorResult(ex);
            }
        }

        [WebMethod]
        public OperationResult Move(int srcPageId, int destPageId, string moveType)
        {
            var session = Database.GetCurrentSession();

            var srcPage = session.Get<PageItem>(srcPageId);
            var destPage = session.Get<PageItem>(destPageId);

            DropPosition strategy = DropPosition.Over;
            if (moveType == "before")
            {
                strategy = DropPosition.Before;
            }
            else if (moveType == "after")
            {
                strategy = DropPosition.After;
            }

            try
            {
                srcPage.MoveTo(destPage, strategy);
                session.Commit();

                return OperationResult.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                return OperationResult.CreateErrorResult(ex);
            }
        }
    }
}
