using AgenteTickets.AppDataBase.Models;
using FluentNHibernate.Mapping;

namespace AgenteTickets.AppDataBase.Maps
{
    public class SelfbillingConfigMap : ClassMap<SelfbillingConfig>
    {
        public SelfbillingConfigMap()
        {
            Table("selfbilling_config");
            _ = Id(x => x.Id, "id").Not.Nullable();
            _ = Map(x => x.ApiUrl, "api_url").Not.Nullable();
            _ = Map(x => x.ApiUser, "api_user").Not.Nullable();
            _ = Map(x => x.ApiPassword, "api_password").Not.Nullable();
            _ = Map(x => x.Serie, "serie").Not.Nullable();
            _ = Map(x => x.LoadIntervalTicketsSeconds, "load_interval_tickets_seconds").Not.Nullable();
            _ = Map(x => x.TimeLoadCancellationsSeconds, "time_load_cancellations_seconds").Not.Nullable();
            _ = Map(x => x.LoadPreviousDayCancellations, "load_previous_day_cancellations").Not.Nullable();
            _ = Map(x => x.ApiToken, "api_token").Nullable();
            _ = Map(x => x.ApiTokenExp, "api_token_exp").Nullable();
        }
    }
}
