using AgenteTickets.AppDataBase.Models;
using FluentNHibernate.Mapping;

namespace AgenteTickets.AppDataBase.Maps
{
    public class PaymentMethodConfigMap: ClassMap<PaymentMethodConfig>
    {
        public PaymentMethodConfigMap()
        {
            Table("payment_method_config");
            _ = Id(x => x.Id, "id").Not.Nullable();
            _ = Map(x => x.POSCode, "pos_code").Not.Nullable();
            _ = Map(x => x.Description, "description").Not.Nullable();
            _ = Map(x => x.TaxCode, "tax_code").Not.Nullable();
        }
    }
}
