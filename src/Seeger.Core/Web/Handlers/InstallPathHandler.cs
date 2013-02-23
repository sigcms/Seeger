using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.Handlers
{
    class InstallPathHandler : IRequestHandler
    {
        public static readonly InstallPathHandler Instance = new InstallPathHandler();

        public void Handle(RequestHandlerContext context)
        {
            string installPath = InstallationInfo.InstallPath;

            if (installPath.Length > 0 && installPath != "/" && context.TargetPath.IgnoreCaseStartsWith(InstallationInfo.InstallPath))
            {
                if (context.TargetPath.Length == InstallationInfo.InstallPath.Length)
                {
                    context.TargetPath = "/";
                }
                else
                {
                    context.TargetPath = context.TargetPath.Substring(InstallationInfo.InstallPath.Length);
                }
            }

            AlwaysIgnoredPathHandler.Instance.Handle(context);
        }
    }
}
