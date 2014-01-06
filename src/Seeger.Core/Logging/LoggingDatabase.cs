using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using Seeger.Data;
using Seeger.Data.Mapping;
using Seeger.Data.Mapping.Impl;
using Seeger.Data.Mapping.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seeger.Logging
{
    class LoggingDatabase
    {
        static readonly Lazy<ISessionFactory> _sessionFactory;

        public static ISessionFactory SessionFactory
        {
            get
            {
                return _sessionFactory.Value;
            }
        }

        static LoggingDatabase()
        {
            _sessionFactory = new Lazy<ISessionFactory>(() =>
            {
                var config = new Configuration();

                config.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlKeyWords, "auto-quote");
                config.Configure(Database.ConfigurationFilePath);

                var mapperFactory = new AttributeMapperFactory();
                mapperFactory.RegisterMappers(new[] { Assembly.GetExecutingAssembly() });

                config.AddMapping(new ConventionMappingCompiler("cms", mapperFactory).AddEntityTypes(new[] { typeof(LogEntry) }).CompileMapping());

                return config.BuildSessionFactory();
            }, true);
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static void Close()
        {
            if (_sessionFactory.IsValueCreated)
            {
                _sessionFactory.Value.Dispose();
            }
        }
    }
}
