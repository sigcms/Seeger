using NHibernate.Mapping.ByCode.Conformist;
using Seeger.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping
{
    class LogEntryMap : ClassMapping<LogEntry>
    {
        public string TableName
        {
            get
            {
                return "cms_" + typeof(LogEntry).Name;
            }
        }

        public LogEntryMap()
        {
            Table(TableName);

            this.HighLowId(c => c.Id, TableName);

            Property(c => c.LoggerName);
            Property(c => c.Level);
            Property(c => c.Message);
            Property(c => c.Exception);
            Property(c => c.UtcTimestamp);

            Component(c => c.Operator, m =>
            {
                m.Property(x => x.Id, x => x.Column("Operator_Id"));
                m.Property(x => x.UserName, x => x.Column("Operator_UserName"));
                m.Property(x => x.Nick, x => x.Column("Operator_Nick"));
            });
        }
    }
}
