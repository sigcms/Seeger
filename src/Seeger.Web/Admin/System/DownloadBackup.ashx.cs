using Seeger.Logging;
using Seeger.Text.Markup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Seeger.Web.UI.Admin._System
{
    public class DownloadBackup : AuthRequiredHttpHandler
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        protected override bool ValidateAccess(Seeger.Security.User user)
        {
            return user.HasPermission(null, "System", "DbBackup");
        }

        protected override void DoProcessRequest(System.Web.HttpContext context)
        {
            var file = context.Request.QueryString["file"];
            var path = Server.MapPath("/App_Data/Backups/" + file);

            _log.Info(UserReference.From(AdminSession.User), "Download database backup".WrapWithTag(Tags.T) + ": " + file);

            context.Response.ContentType = "application/zip, application/octet-stream";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + file);
            context.Response.TransmitFile(path);
        }
    }
}