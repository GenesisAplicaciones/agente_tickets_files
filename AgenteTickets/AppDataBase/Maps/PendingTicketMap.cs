using AgenteTickets.AppDataBase.Models;
using FluentNHibernate.Mapping;

namespace AgenteTickets.AppDataBase.Maps
{
    public class PendingTicketMap : ClassMap<PendingTicket>
    {
        public PendingTicketMap()
        {
            Table("pending_tickets");
            _ = Id(x => x.Id, "id").GeneratedBy.Assigned();
            _ = Map(x => x.JsonTicket, "json_ticket").Not.Nullable();
        }
    }
}
