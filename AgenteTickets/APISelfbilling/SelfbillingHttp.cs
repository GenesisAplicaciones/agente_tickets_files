using AgenteTickets.APISelfbilling.Services;
using AgenteTickets.AppDataBase.DAO;
using AgenteTickets.Models;
using AgenteTickets.Persistence;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace AgenteTickets.APISelfbilling
{
    public static class SelfbillingHttp
    {
        private static RestClient client;
        public static RestClient Client
        {
            get
            {
                if (client == null)
                {
                    ResfreshBaseUrl();
                }

                return client;
            }
        }

        public static void ResfreshBaseUrl(string baseUrl)
        {
            JsonNetSerializer.DefaultSettings.NullValueHandling = NullValueHandling.Include;
            client = new RestClient(baseUrl, configureSerialization: s => s.UseSerializer(() => new JsonNetSerializer()));
        }

        public static void ResfreshBaseUrl()
        {
            if (AppInstances.SelfbillingConfig != null)
            {
                ResfreshBaseUrl(AppInstances.SelfbillingConfig.ApiUrl);
            }

        }

        public static void SetAuthorization(string token)
        {
            _ = Client?.DefaultParameters.RemoveParameter("Authorization", ParameterType.HttpHeader);
            _ = Client?.AddDefaultHeader("Authorization", token);
        }

        public static void RefreshToken()
        {
            if (Client == null)
            {
                return;
            }

            if (AppInstances.SelfbillingConfig.TokenIsExpired)
            {
                ResfreshBaseUrl();
                SelfbillingResponse response = AuthService.GenerateToken(AppInstances.SelfbillingConfig.ApiUser, AppInstances.SelfbillingConfig.ApiPassword);

                if (response.IsSuccessful)
                {
                    AppInstances.SelfbillingConfig.ApiToken = response.Token;
                    AppInstances.SelfbillingConfig.ApiTokenExp = response.DecodedToken.Exp;
                }
                else
                {
                    AppInstances.SelfbillingConfig.ApiToken = "";
                    AppInstances.SelfbillingConfig.ApiTokenExp = null;
                }

                _ = SelfbillingConfigDAO.Update(AppInstances.SelfbillingConfig);
            }

            SetAuthorization(AppInstances.SelfbillingConfig.ApiToken);
        }
    }
}
