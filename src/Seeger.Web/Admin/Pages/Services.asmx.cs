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
                var session = Database.GetCurrentSession();
                var page = session.Get<PageItem>(pageId);

                if (page.Parent == null)
                {
                    session.Delete(page);
                }
                else
                {
                    page.Parent.Pages.Remove(page);
                }

                session.Commit();

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

            var from = session.Get<PageItem>(srcPageId);
            var to = session.Get<PageItem>(destPageId);

            var strategy = DropPosition.Over;

            if (!String.IsNullOrEmpty(moveType))
            {
                strategy = (DropPosition)Enum.Parse(typeof(DropPosition), moveType, true);
            }

            try
            {
                var service = new PageMovementService(session);
                service.Move(from, to, strategy);
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
