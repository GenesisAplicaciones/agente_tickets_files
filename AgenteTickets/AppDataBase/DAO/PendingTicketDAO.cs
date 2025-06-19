using AgenteTickets.AppDataBase.Models;
using AgenteTickets.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgenteTickets.AppDataBase.DAO
{
    public class PendingTicketDAO : DAO<PendingTicket>
    {
        public static bool Create(Ticket ticket)
        {
            return Create(new PendingTicket
            {
                Id = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Ticket = ticket
            });
        }

        public static List<PendingTicket> Get(long lastId, int take)
        {
            using (ISession session = AppDBInstance.OpenSession())
            {
                return (from x in session.Query<PendingTicket>()
                        where x.Id > lastId
                        orderby x.Id ascending
                        select x).Take(take).ToList();
            }
        }
    }
}
