using Ionic.Zip;
using Seeger.Data;
using Seeger.Data.Backup;
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
            var folder = HostingEnvironment.MapPath("/App_Data/Backups");
            IOUtil.EnsureDirectoryCreated(folder);

            var backupPath = Path.Combine(folder, DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".bak");

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
                zip.Save(Path.Combine(folder, Path.GetFileNameWithoutExtension(backupPath) + ".zip"));
            }
            finally
            {
                IOUtil.EnsureFileDeleted(backupPath);
            }
        }

        [WebMethod, ScriptMethod]
        public static void Delete(string file)
        {
            var filePath = HostingEnvironment.MapPath("/App_Data/Backups/" + file);
            IOUtil.EnsureFileDeleted(filePath);
        }
    }
}