using NHibernate;
using Seeger.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Logging
{
    public class DefaultLogger : Logger
    {
        public Func<ISession> OpenSession { get; private set; }

        public DefaultLogger(string name)
            : this(name, () => LoggingDatabase.OpenSession())
        {
        }

        public DefaultLogger(string name, Func<ISession> openSession)
            : base(name)
        {
            Require.NotNull(openSession, "openSession");
            OpenSession = openSession;
        }

        protected override void DoLog(UserReference @operator, LogLevel level, Exception exception, string message)
        {
            var entry = CreateLogEntry(@operator, level, exception, message);

            using (var session = OpenSession())
            {
                session.Save(entry);
                session.Commit();
            }
        }

        private LogEntry CreateLogEntry(UserReference @operator, LogLevel level, Exception exception, string message)
        {
            var entry = new LogEntry
            {
                LoggerName = Name,
                Level = level,
                Operator = @operator,
                Message = message
            };

            if (exception != null)
            {
                entry.Exception = ExceptionPrinter.Print(exception);
            }

            return entry;
        }
    }
}
