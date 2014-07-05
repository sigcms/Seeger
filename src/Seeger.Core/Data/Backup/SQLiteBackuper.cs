using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Hosting;

namespace Seeger.Data.Backup
{
    public class SQLiteBackuper : IDbBackuper
    {
        static readonly Regex _dataSourcePattern = new Regex(@"Data Source=(?<ds>[^;]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string BackupFileExtension
        {
            get
            {
                return ".sqlite";
            }
        }

        public string Backup(DbConnection connection, string backupDirectory, string backupFileNameWithoutExtension)
        {
            var dbPath = GetDatabasePath(connection.ConnectionString.Trim());
            if (String.IsNullOrEmpty(dbPath))
                throw new InvalidOperationException("Cannot resolve SQLite database file path from connection string.");

            var backupPath = Path.Combine(backupDirectory, backupFileNameWithoutExtension + BackupFileExtension);
            File.Copy(dbPath, backupPath, true);

            return backupPath;
        }

        private string GetDatabasePath(string connectionString)
        {
            var match = _dataSourcePattern.Match(connectionString);
            if (match.Success)
            {
                var dataSource = match.Groups["ds"].Value;
                var appDataPath = HostingEnvironment.MapPath("~/App_Data/");
                if (!appDataPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    appDataPath += Path.DirectorySeparatorChar;
                }

                dataSource = dataSource.Replace("|DataDirectory|", appDataPath);

                return dataSource;
            }

            return null;
        }
    }
}
