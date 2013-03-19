using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Logging
{
    public abstract class Logger
    {
        private NLog.Logger _fallbackLogger = null;
        private readonly object _lock = new object();

        protected NLog.Logger FallbackLogger
        {
            get
            {
                if (_fallbackLogger == null)
                {
                    lock (_lock)
                    {
                        if (_fallbackLogger == null)
                        {
                            _fallbackLogger = NLog.LogManager.GetLogger(Name);
                        }
                    }
                }

                return _fallbackLogger;
            }
        }

        public string Name { get; private set; }

        protected Logger(string name)
        {
            Require.NotNullOrEmpty(name, "name");
            Name = name;
        }

        public virtual bool IsEnabled(LogLevel level)
        {
            return FallbackLogger.IsEnabled(GetNLogLevel(level));
        }

        public void Debug(UserReference @operator, string message)
        {
            Log(@operator, LogLevel.Debug, message);
        }

        public void Debug(UserReference @operator, string message, params object[] args)
        {
            Log(@operator, LogLevel.Debug, message, args);
        }

        public void DebugException(Exception exception, UserReference @operator, string message)
        {
            LogException(@operator, LogLevel.Debug, exception, message);
        }

        public void DebugException(Exception exception, UserReference @operator, string message, params object[] args)
        {
            LogException(@operator, LogLevel.Debug, exception, message, args);
        }

        public void Info(UserReference @operator, string message)
        {
            Log(@operator, LogLevel.Info, message);
        }

        public void Info(UserReference @operator, string message, params object[] args)
        {
            Log(@operator, LogLevel.Info, message, args);
        }

        public void InfoException(Exception exception, UserReference @operator, string message)
        {
            LogException(@operator, LogLevel.Info, exception, message);
        }

        public void InfoException(Exception exception, UserReference @operator, string message, params object[] args)
        {
            LogException(@operator, LogLevel.Info, exception, message, args);
        }

        public void Warn(UserReference @operator, string message)
        {
            Log(@operator, LogLevel.Warn, message);
        }

        public void Warn(UserReference @operator, string message, params object[] args)
        {
            Log(@operator, LogLevel.Warn, message, args);
        }

        public void WarnException(UserReference @operator, Exception exception, string message)
        {
            LogException(@operator, LogLevel.Warn, exception, message);
        }

        public void WarnException(UserReference @operator, Exception exception, string message, params object[] args)
        {
            LogException(@operator, LogLevel.Warn, exception, message, args);
        }

        public void Error(UserReference @operator, string message)
        {
            Log(@operator, LogLevel.Error, message);
        }

        public void Error(UserReference @operator, string message, params object[] args)
        {
            Log(@operator, LogLevel.Error, message, args);
        }

        public void ErrorException(UserReference @operator, Exception exception, string message)
        {
            LogException(@operator, LogLevel.Error, exception, message);
        }

        public void ErrorException(UserReference @operator, Exception exception, string message, params object[] args)
        {
            LogException(@operator, LogLevel.Error, exception, message, args);
        }

        public void Log(UserReference @operator, LogLevel level, string message)
        {
            if (!IsEnabled(level)) return;

            try
            {
                DoLog(@operator, level, null, message);
            }
            catch
            {
                FallbackLogger.Log(GetNLogLevel(level), message);
            }
        }

        public void Log(UserReference @operator, LogLevel level, string message, params object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Log(@operator, level, message);
            }
            else
            {
                Log(@operator, level, String.Format(message, args));
            }
        }

        public void LogException(UserReference @operator, LogLevel level, Exception exception, string message)
        {
            if (!IsEnabled(level)) return;

            try
            {
                DoLog(@operator, level, exception, message);
            }
            catch
            {
                FallbackLogger.LogException(GetNLogLevel(level), message, exception);
            }
        }

        public void LogException(UserReference @operator, LogLevel level, Exception exception, string message, params object[] args)
        {
            if (args == null || args.Length == 0)
            {
                LogException(@operator, level, exception, message);
            }
            else
            {
                LogException(@operator, level, exception, String.Format(message, args));
            }
        }

        protected abstract void DoLog(UserReference @operator, LogLevel level, Exception exception, string message);

        protected NLog.LogLevel GetNLogLevel(LogLevel level)
        {
            if (level == LogLevel.Debug)
            {
                return NLog.LogLevel.Debug;
            }
            if (level == LogLevel.Info)
            {
                return NLog.LogLevel.Info;
            }
            if (level == LogLevel.Warn)
            {
                return NLog.LogLevel.Warn;
            }
            if (level == LogLevel.Error)
            {
                return NLog.LogLevel.Error;
            }
            if (level == LogLevel.Fatal)
            {
                return NLog.LogLevel.Fatal;
            }

            return NLog.LogLevel.Debug;
        }
    }
}
