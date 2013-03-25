using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.Data.Backup
{
    public class MssqlBackuper : IDbBackuper
    {
        public string BackupFileExtension
        {
            get
            {
                return ".bak";
            }
        }

        public string Backup(DbConnection connection, string backupDirectory, string backupFileNameWithoutExtension)
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            var backupPath = Path.Combine(backupDirectory, backupFileNameWithoutExtension + BackupFileExtension);

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "BACKUP DATABASE [" + connection.Database + "] TO DISK = N'" + backupPath + "'";
                cmd.ExecuteNonQuery();
            }

            return backupPath;
        }
    }
}
