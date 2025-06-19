using AgenteTickets.AppDataBase.Enums;
using AgenteTickets.AppDataBase.Models;
using FluentNHibernate.Mapping;

namespace AgenteTickets.AppDataBase.Maps
{
    public class TicketLogMap : ClassMap<TicketLog>
    {
        public TicketLogMap()
        {
            Table("ticket_log");
            _ = Id(x => x.Id, "id").Not.Nullable();
            _ = Map(x => x.TicketSerie, "ticket_serie").Not.Nullable();
            _ = Map(x => x.TicketFolio, "ticket_folio").Not.Nullable();
            _ = Map(x => x.TicketDate, "ticket_date").Not.Nullable();
            _ = Map(x => x.TicketAmount, "ticket_amount")
                .CustomSqlType("NUMERIC")
                .Precision(18)
                .Scale(2)
                .Not.Nullable();
            _ = Map(x => x.TicketStatusCode, "ticket_status_code").Nullable();
            _ = Map(x => x.TypeMethod, "type_method").CustomType<TypeMethod>().Not.Nullable();
            _ = Map(x => x.TypeAction, "type_action").CustomType<TypeAction>().Not.Nullable();
            _ = Map(x => x.StatusAction, "status_action").CustomType<StatusAction>().Not.Nullable();
            _ = Map(x => x.ApiResponseCode, "api_response_code").Nullable();
            _ = Map(x => x.Message, "message").Nullable();
        }
    }
}
