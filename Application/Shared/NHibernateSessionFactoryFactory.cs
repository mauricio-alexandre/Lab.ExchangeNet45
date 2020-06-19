using System.Data;
using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Connection;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;

namespace Lab.ExchangeNet45.Application.Shared
{
    public class NHibernateSessionFactoryFactory
    {
//        public NHibernateSessionFactoryFactory()
//        {
//#if DEBUG
//            NHibernateProfiler.Initialize();
//#endif
//        }

        private static FluentConfiguration CreateConfiguration()
        {
            return Fluently
                .Configure()
                .CurrentSessionContext<ThreadStaticSessionContext>()
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<NHibernateSessionFactoryFactory>();
                });
        }

        public ISessionFactory CreateSqLite()
        {
            string databaseFilePath = Path.Combine(Path.GetTempPath(), "Lab.ExchangeNet45.sqlite");

            return CreateConfiguration()
                .Database
                (
                    SQLiteConfiguration.Standard
                        .ConnectionString($"data source={databaseFilePath};Version=3;New=True;BinaryGuid=False;PRAGMA synchronous=off;")
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

                    var schemaUpdate = new SchemaUpdate(config);
                    schemaUpdate.Execute(true, true);
                })
                .BuildSessionFactory();
        }
    }
}
