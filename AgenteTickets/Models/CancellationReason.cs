using Newtonsoft.Json;

namespace AgenteTickets.Models
{
    public class CancellationReason
    {
        [JsonProperty("usuario")]
        public string User { get; set; }

        [JsonProperty("motivo")]
        public string Message { get; set; }
    }
}
