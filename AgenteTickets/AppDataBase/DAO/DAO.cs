using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace AgenteTickets.AppDataBase.DAO
{
    public class DAO<Entity>
    {
        public static bool Create(Entity entity)
        {
            using (ISession session = AppDBInstance.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    _ = session.Save(entity);
                    transaction.Commit();
                    return true;
                }
            }
        }

        public static Entity Find<Id>(Id id)
        {
            using (ISession session = AppDBInstance.OpenSession())
            {
                return session.Get<Entity>(id);
            }
        }

        public static bool Update(Entity entity)
        {
            using (ISession session = AppDBInstance.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(entity);
                    transaction.Commit();
                    return true;
                }

            }
        }

        public static bool Delete<Id>(Id id)
        {
            using (ISession session = AppDBInstance.OpenSession())
            {
                Entity entity = session.Get<Entity>(id);

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(entity);
                    transaction.Commit();
                    return true;
                }
            }
        }

        public static int Count()
        {
            using (ISession session = AppDBInstance.OpenSession())
            {
                return session.Query<Entity>().Count();
            }
        }

        public static List<Entity> Get(int take, int skip = 0)
        {
            using (ISession session = AppDBInstance.OpenSession())
            {
                return session.Query<Entity>().Skip(skip).Take(take).ToList();
            }
        }

        public static List<Entity> Get()
        {
            using (ISession session = AppDBInstance.OpenSession())
            {
                return session.Query<Entity>().ToList();
            }
        }
    }
}
