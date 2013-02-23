using System;
using System.Web;

namespace Seeger.Web.Handlers
{
    public interface IRequestHandler
    {
        void Handle(RequestHandlerContext context);
    }
}
