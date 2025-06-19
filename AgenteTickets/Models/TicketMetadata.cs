using Newtonsoft.Json;

namespace AgenteTickets.Models
{
    public class TicketMetadata
    {
        [JsonProperty("serie")]
        public string Serie { get; set; }

        [JsonProperty("folio")]
        public string Folio { get; set; }

        [JsonProperty("importe")]
        public decimal Amount { get; set; }

        [JsonProperty("descuento")]
        public decimal Discount { get; set; }

        [JsonProperty("fecha_ticket")]
        public string TicketDate { get; set; }

        [JsonProperty("forma_pago")]
        public string PaymentMethod { get; set; }

        [JsonProperty("QR")]
        public string Qr { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
