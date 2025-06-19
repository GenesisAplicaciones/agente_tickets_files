using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace AgenteTickets.Utils
{
    public static class JWT
    {
        public static T Decode<T>(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            if (handler.ReadToken(token) is JwtSecurityToken jsonToken)
            {
                string jsonPayload = JsonConvert.SerializeObject(jsonToken.Payload);
                return JsonConvert.DeserializeObject<T>(jsonPayload);
            }

            return default;
        }

        public static long UnixTimestamp
        {
            get => (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static long UnixTimestampConvert(DateTime date)
        {
            return ((DateTimeOffset)date.ToUniversalTime()).ToUnixTimeSeconds();
        }
    }
}
