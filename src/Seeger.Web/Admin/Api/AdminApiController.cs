using Seeger.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Seeger.Web.UI.Admin.Api
{
    [RoutePrefix("api/admin"), Authorize]
    public class AdminApiController : ApiController
    {
    }
}