using System;
using System.Data;
using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using Lab.ExchangeNet45.Application.Shared;
using NHibernate;
using NHibernate.Connection;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Application.Tests.Persistence
{
    [TestFixture]
    public abstract class NHibernateSqLiteTestFixture
    {
        private string DatabaseFilePath { get; set; }

        protected ISessionFactory SessionFactory { get; private set; }

        [SetUp]
        public void BuildSessionFactory()
        {
            DatabaseFilePath = Path.Combine(Path.GetTempPath(), "Lab.ExchangeNet45.Tests.sqlite");
#if DEBUG
            NHibernateProfiler.Initialize();
#endif
            SessionFactory = Fluently
                .Configure()
                .CurrentSessionContext<ThreadStaticSessionContext>()
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<NHibernateSessionFactoryFactory>();
                })
                .Database
                (
                    SQLiteConfiguration.Standard
                        .ConnectionString($"data source={DatabaseFilePath};Version=3;New=True;BinaryGuid=False;PRAGMA synchronous=off;")
                        .Driver<SQLite20Driver>()
                        .Dialect<SQLiteDialect>()
                        .Provider<DriverConnectionProvider>()
                        .QuerySubstitutions("true 1, false 0")
                        .ShowSql()
                        .FormatSql()
                        .AdoNetBatchSize(250)
                        .IsolationLevel(IsolationLevel.ReadCommitted)
                )
                .ExposeConfiguration(config =>
                {
                    config.Properties.Add("hbm2ddl.keywords", "none");

                    var schemaExport = new SchemaExport(config);
                    schemaExport.Create(true, true);
                })
                .BuildSessionFactory();
        }

        [TearDown]
        public void DisposeSessionFactory()
        {
            SessionFactory.Dispose();
            try
            {
                File.Delete(DatabaseFilePath);
            }
            catch
            {
                // ignored
            }
        }

        protected void ExecuteSessionTransactionScope(Action<ISession> scope)
        {
            using (ISession session = SessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                scope.Invoke(session);
                transaction.Commit();
            }
        }
    }
}
