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
        void Backup(DbConnection connection, string backupSavePath);
    }

    public static class DbBackupers
    {
        public static IDbBackuper Get(string driverName)
        {
            if (driverName.StartsWith("SQLite"))
            {
                return new SQLiteBackuper();
            }

            return new MssqlBackuper();
        }
    }
}
