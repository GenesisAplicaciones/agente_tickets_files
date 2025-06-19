using Newtonsoft.Json;

namespace AgenteTickets.Models
{
    public class Payment
    {
        [JsonProperty("id_pago")]
        public string PaymentId { get; set; }

        [JsonProperty("nombre")]
        public string Name { get; set; }

        [JsonProperty("forma_pago")]
        public string PaymentMethod { get; set; }

        [JsonProperty("importe")]
        public decimal Amount { get; set; }

        [JsonProperty("propina")]
        public decimal? Tip { get; set; }
    }
}
