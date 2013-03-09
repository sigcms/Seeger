using System;
using System.IO;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using Seeger.Data.Mapping;
using Seeger.Data.Context;
using NLog;
using Seeger.Plugins;

namespace Seeger.Data
{
    public static class Database
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private static Configuration _config;

        public static ISessionFactory SessionFactory { get; private set; }

        public static string Dialect
        {
            get
            {
                return _config.GetProperty(NHibernate.Cfg.Environment.Dialect);
            }
        }

        public static string Driver
        {
            get
            {
                return _config.GetProperty(NHibernate.Cfg.Environment.ConnectionDriver);
            }
        }

        public static string ConnectionString
        {
            get
            {
                return _config.GetProperty(NHibernate.Cfg.Environment.ConnectionString);
            }
        }

        public static void Initialize()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\nhibernate.config");
            Initialize(path);
        }

        public static void Initialize(string nhibernateConfigFilePath)
        {
            Require.NotNullOrEmpty(nhibernateConfigFilePath, "nhibernateConfigFilePath");
            Require.That(File.Exists(nhibernateConfigFilePath), "Cannot find NHibernate configuration file.");

            _log.Debug("Initializing NhSessionManager");

            var config = new Configuration();

            config.Configure(nhibernateConfigFilePath);
            config.AddMapping(ByCodeMappingLoader.LoadMappingFrom(Assembly.GetExecutingAssembly()));

            var sessionFactory = SessionFactory;
            if (sessionFactory != null)
            {
                sessionFactory.Dispose();
            }

            foreach (var plugin in PluginManager.EnabledPlugins)
            {
                var mappingProviderType = NhMappingProviders.GetMappingProviderType(plugin.Name);
                if (mappingProviderType != null)
                {
                    var mappingProvider = (INhMappingProvider)Activator.CreateInstance(mappingProviderType);
                    foreach (var mapping in mappingProvider.GetMappings())
                    {
                        config.AddMapping(mapping);
                    }
                }
            }

            sessionFactory = config.BuildSessionFactory();

            _config = config;
            SessionFactory = sessionFactory;
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static ISession GetCurrentSession()
        {
            return NhSessionContexts.Current.GetCurrentSession();
        }

        public static void CloseCurrentSession()
        {
            NhSessionContexts.Current.CloseCurrentSession();
        }
    }
}
