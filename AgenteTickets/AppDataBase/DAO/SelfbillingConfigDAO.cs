using AgenteTickets.AppDataBase.Models;

namespace AgenteTickets.AppDataBase.DAO
{
    public class SelfbillingConfigDAO : DAO<SelfbillingConfig>
    {
        public static SelfbillingConfig Find()
        {
            return Find(1);
        }
    }
}
