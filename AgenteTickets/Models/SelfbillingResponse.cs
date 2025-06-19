using Newtonsoft.Json;
using System.Collections.Generic;

namespace AgenteTickets.Models
{
    public class SelfbillingResponse<T>
    {
        public SelfbillingResponse()
        {
            Results = new List<T>();
        }

        [JsonProperty("codigo")]
        public int? Code { get; set; }

        [JsonProperty("mensaje")]
        public string Message { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("resultado")]
        public T Result { get; set; }

        [JsonProperty("resultados")]
        public List<T> Results { get; set; }

        public bool IsSuccessful => Code == 200 || Code == 205;

        public SelfbillingJWTPayload DecodedToken { get; set; }
    }

    public class SelfbillingResponse : SelfbillingResponse<string>
    {
        public SelfbillingResponse() : base() { }
    }
}
