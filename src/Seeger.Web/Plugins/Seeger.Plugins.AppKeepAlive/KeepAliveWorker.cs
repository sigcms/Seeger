using Seeger.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace Seeger.Plugins.AppKeepAlive
{
    public static class KeepAliveWorker
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        static Thread _thread;
        static AutoResetEvent _event;
        static bool _needToStop;
        static string _url;
        static TimeSpan _interval;

        public static TimeSpan Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                _interval = value;
            }
        }

        public static string Url
        {
            get
            {
                return _url;
            }
            set
            {
                Require.NotNullOrEmpty(value, "value");
                _url = value;
            }
        }

        public static bool IsRunning { get; private set; }

        static KeepAliveWorker()
        {
            Interval = TimeSpan.FromMinutes(10);
        }

        public static void Start(string url, TimeSpan interval)
        {
            Require.NotNullOrEmpty(url, "url");

            if (IsRunning) return;

            _url = url;
            _interval = interval;

            _event = new AutoResetEvent(false);
            _thread = new Thread(ThreadStart);
            _thread.Start();
            IsRunning = true;

            _log.Info(UserReference.System(), "Keep alive worker: Worker thread started");
        }

        public static void Stop(bool waitUntilStopped = false)
        {
            if (!IsRunning) return;

            _log.Info(UserReference.System(), "Keep alive worker: Stop command received");

            _needToStop = true;
            _event.Set();

            if (waitUntilStopped)
            {
                _thread.Join();
            }
        }

        static void ThreadStart()
        {
            while (true)
            {
                _event.WaitOne(Interval);

                if (_needToStop)
                {
                    break;
                }
                else
                {
                    MakeRequest();
                }
            }

            IsRunning = false;

            _event.Dispose();
            _event = null;
            _thread = null;

            _log.Info(UserReference.System(), "Keep alive worker: Worker thread stopped");
        }

        static void MakeRequest()
        {
            try
            {
                var url = Url;
                
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.DownloadString(url);
                }

                _log.Info(UserReference.System(), "Keep alive worker: Completed a new http request to " + url);
            }
            catch (Exception ex)
            {
                _log.ErrorException(UserReference.System(), ex, "Keep alive worker: Http request failed.");
            }
        }
    }
}