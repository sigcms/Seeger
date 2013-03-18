using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.Data.Backup
{
    public class SQLiteBackuper : IDbBackuper
    {
        public void Backup(DbConnection connection, string backupSavePath)
        {
            var dbPath = connection.DataSource;
            File.Copy(dbPath, backupSavePath, true);
        }
    }
}
