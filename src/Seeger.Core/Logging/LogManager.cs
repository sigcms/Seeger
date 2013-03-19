using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Seeger.Logging
{
    public static class LogManager
    {
        static readonly ConcurrentDictionary<string, Logger> _loggers = new ConcurrentDictionary<string, Logger>();

        public static Logger GetCurrentClassLogger()
        {
            var stackFrame = new StackFrame(1, false);
            return GetLogger(stackFrame.GetMethod().DeclaringType.FullName);
        }

        public static Logger GetLogger(string name)
        {
            return _loggers.GetOrAdd(name, n => new DefaultLogger(n));
        }
    }
}
