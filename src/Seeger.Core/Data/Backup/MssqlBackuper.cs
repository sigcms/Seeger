using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Seeger.Data.Backup
{
    public class MssqlBackuper : IDbBackuper
    {
        public void Backup(DbConnection connection, string backupSavePath)
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "BACKUP DATABASE [" + connection.Database + "] TO DISK = N'" + backupSavePath + "'";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
