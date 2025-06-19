using AgenteTickets.Models;
using Newtonsoft.Json;

namespace AgenteTickets.AppDataBase.Models
{
    public class PendingTicket
    {
        public virtual long Id { get; set; }
        public virtual string JsonTicket { get; set; }

        public virtual Ticket Ticket
        {
            get => JsonConvert.DeserializeObject<Ticket>(JsonTicket);
            set => JsonTicket = JsonConvert.SerializeObject(value);
        }

    }
}
