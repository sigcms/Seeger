using Seeger.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace Seeger.Plugins.Comments
{
    public class CommentsPlugin : PluginBase
    {
        public override void OnInstall(PluginLifecycleContext context)
        {
            using (var session = Database.OpenSession())
            {
                var conn = session.Connection;
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = LoadInstallSql();

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        // It might fail if the table already exists. Just ignore the exception for now.
                    }
                }
            }
        }

        private string LoadInstallSql()
        {
            var path = HostingEnvironment.MapPath("~/Plugins/" + Strings.PluginName + "/db/install.sql");
            return File.ReadAllText(path, Encoding.UTF8);
        }
    }
}