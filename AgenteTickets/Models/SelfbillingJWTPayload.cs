using AgenteTickets.Utils;
using Newtonsoft.Json;
using System;

namespace AgenteTickets.Models
{
    public class SelfbillingJWTPayload
    {
        [JsonProperty("timestamp")]
        public int TimeStamp { get; set; }

        [JsonProperty("expiracion")]
        public DateTime Expiration { get; set; }

        [JsonProperty("data")]
        public User User { get; set; }

        [JsonIgnore]
        public long Exp => JWT.UnixTimestampConvert(Expiration);
    }
}
