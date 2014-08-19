using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;

namespace Seeger.Web.Http
{
    public class HttpErrorResponse
    {
        public string Message { get; set; }
    }

    public class HandleErrorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;
            if (exception != null)
            {
                var request = actionExecutedContext.Request;
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new HttpErrorResponse
                {
                    Message = exception.Message
                });
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
