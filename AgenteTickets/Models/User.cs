using Newtonsoft.Json;

namespace AgenteTickets.Models
{
    public class User
    {
        [JsonProperty("idUsuario")]
        public int UserId { get; set; }

        [JsonProperty("nombre")]
        public string Name { get; set; }
    }
}
