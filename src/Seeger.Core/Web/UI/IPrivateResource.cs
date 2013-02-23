using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Seeger.Security;

namespace Seeger.Web.UI
{
    public interface IPrivateResource
    {
        bool VerifyAccess(User user);
    }
}
