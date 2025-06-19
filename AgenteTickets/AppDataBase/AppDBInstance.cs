using AgenteTickets.AppDataBase.Maps;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.IO;

namespace AgenteTickets.AppDataBase
{
    public static class AppDBInstance
    {
        public static string DBFileName => "AgenteTickets.sqlite";
        private static string DBFileBackup => $"{DBFileName}.backup";
        public static bool DBFileExist
        {
            get
            {
                if (File.Exists(DBFileName))
                {
                    return true;
                }
                else
                {
                    return RestoreDBFile();
                }
            }
        }

        public static bool CreateBackupDBFile()
        {
            if (File.Exists(DBFileName))
            {
                File.Copy(DBFileName, DBFileBackup, true);
                return true;
            }

            return false;
        }

        public static bool RestoreDBFile()
        {
            if (File.Exists(DBFileBackup))
            {
                File.Copy(DBFileBackup, DBFileName, true);
                return true;
            }

            DeleteDBFile();

            return false;
        }

        public static void DeleteDBFile()
        {
            if (File.Exists(DBFileName))
            {
                File.Delete(DBFileName);
            }
        }

        private static ISessionFactory sessionFactory;
        public static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                {
                    sessionFactory = Fluently.Configure()
                        .Database(SQLiteConfiguration.Standard
                        .ConnectionString($"Data Source={DBFileName};Version=3;")
                        .ShowSql())
                        .Mappings(m => m.FluentMappings
                            .Add<FileReaderConfigMap>()
                            .Add<SelfbillingConfigMap>()
                            .Add<TicketLogMap>()
                            .Add<PaymentMethodConfigMap>()
                            .Add<PendingTicketMap>())
                        .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                        .BuildSessionFactory();
                }
                return sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
