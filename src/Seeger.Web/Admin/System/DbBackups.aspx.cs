﻿using Ionic.Zip;
using Seeger.Data;
using Seeger.Data.Backup;
using Seeger.Globalization;
using Seeger.Logging;
using Seeger.Text.Markup;
using Seeger.Utils;
using Seeger.Web.UI.Grid;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.Admin._System
{
    public partial class DbBackups : AjaxGridPageBase
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public override bool VerifyAccess(Seeger.Security.User user)
        {
            return user.HasPermission(null, "System", "DbBackup");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod, ScriptMethod]
        public static void NewBackup()
        {
            var backupFileNameWithoutExtension = DateTime.Now.ToString("yyyy-MM-dd-HHmmss");

            _log.Info(UserReference.From(AdminSession.Current.User), "Backup database to".WrapWithTag(Tags.T) + ": " + backupFileNameWithoutExtension + ".zip");

            var folder = HostingEnvironment.MapPath("/App_Data/Backups");
            IOUtil.EnsureDirectoryCreated(folder);

            var backupPath = Path.Combine(folder, backupFileNameWithoutExtension + ".bak");

            try
            {
                var backuper = DbBackupers.Get(Database.Driver);
                using (var session = Database.OpenSession())
                {
                    backuper.Backup((DbConnection)session.Connection, backupPath);
                }

                // zip the backup file
                var zip = new ZipFile();
                zip.AddFile(backupPath);
                zip.Save(Path.Combine(folder, backupFileNameWithoutExtension + ".zip"));
            }
            finally
            {
                IOUtil.EnsureFileDeleted(backupPath);
            }
        }

        [WebMethod, ScriptMethod]
        public static void Delete(string file)
        {
            _log.Info(UserReference.From(AdminSession.Current.User), "Delete database backup".WrapWithTag(Tags.T) + ": " + file);

            var filePath = HostingEnvironment.MapPath("/App_Data/Backups/" + file);
            IOUtil.EnsureFileDeleted(filePath);
        }
    }
}