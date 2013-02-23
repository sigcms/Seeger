using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Seeger.Data;
using NHibernate.Linq;

namespace Seeger
{
    public class GlobalSettingManager
    {
        private NameValueCollection _settings = new NameValueCollection();

        public bool IsDirty { get; private set; }

        public DefaultSiteInfo DefaultSiteInfo { get; private set; }

        public FrontendSettings FrontendSettings { get; private set; }

        public SmtpSettings Smtp { get; private set; }

        public TaskQueueSettings TaskQueue { get; private set; }

        public string GetValue(string key)
        {
            Require.NotNullOrEmpty(key, "key");

            return _settings[key] ?? String.Empty;
        }

        public T TryGetValue<T>(string key, T defaultValue)
        {
            Require.NotNullOrEmpty(key, "key");

            return _settings.TryGetValue<T>(key, defaultValue);
        }

        public void SetValue(string key, string value)
        {
            Require.NotNullOrEmpty(key, "key");

            if (value == null)
            {
                value = String.Empty;
            }

            _settings[key] = value;

            IsDirty = true;
        }

        #region Submit / Refresh

        private readonly object _lock = new object();

        public void SubmitChanges()
        {
            if (IsDirty)
            {
                lock (_lock)
                {
                    if (IsDirty)
                    {
                        var session = NhSessionManager.GetCurrentSession();

                        foreach (var key in _settings.AllKeys)
                        {
                            var item = session.Get<GlobalSetting>(key);
                            if (item == null)
                            {
                                item = new GlobalSetting(key)
                                {
                                    Value = _settings[key]
                                };

                                session.Save(item);
                            }
                            else
                            {
                                item.Value = _settings[key];
                            }
                        }

                        session.Commit();

                        IsDirty = false;
                    }
                }
            }
        }

        public void Refresh()
        {
            lock (_lock)
            {
                var settings = new NameValueCollection();

                using (var session = NhSessionManager.OpenSession())
                {
                    foreach (var item in session.Query<GlobalSetting>())
                    {
                        settings[item.Key] = item.Value;
                    }
                }

                _settings = settings;

                IsDirty = false;
            }
        }

        #endregion

        #region Instance

        private static readonly Lazy<GlobalSettingManager> _instance;

        public static GlobalSettingManager Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        static GlobalSettingManager()
        {
            _instance = new Lazy<GlobalSettingManager>(() =>
            {
                var manager = new GlobalSettingManager();
                manager.Refresh();
                return manager;
            }, true);
        }

        private GlobalSettingManager()
        {
            DefaultSiteInfo = new DefaultSiteInfo(this);
            FrontendSettings = new FrontendSettings(this);
            Smtp = new SmtpSettings(this);
            TaskQueue = new TaskQueueSettings(this);
        }

        #endregion
    }
}
