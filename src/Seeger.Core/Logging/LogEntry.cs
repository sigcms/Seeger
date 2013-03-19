using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Logging
{
    public class LogEntry
    {
        public virtual int Id { get; set; }

        public virtual string LoggerName { get; set; }

        public virtual LogLevel Level { get; set; }

        public virtual string Message { get; set; }

        public virtual string Exception { get; set; }

        public virtual UserReference Operator { get; set; }

        public virtual DateTime UtcTimestamp { get; set; }

        public LogEntry()
        {
            UtcTimestamp = DateTime.UtcNow;
        }
    }
}
