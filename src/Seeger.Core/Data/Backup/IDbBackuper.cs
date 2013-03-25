using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Seeger.Data.Backup
{
    public interface IDbBackuper
    {
        string BackupFileExtension { get; }

        string Backup(DbConnection connection, string backupDirectory, string backupFileNameWithoutExtension);
    }

    public static class DbBackupers
    {
        public static IDbBackuper Get(string driverName)
        {
            if (driverName.IndexOf("SQLite", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new SQLiteBackuper();
            }

            return new MssqlBackuper();
        }
    }
}
