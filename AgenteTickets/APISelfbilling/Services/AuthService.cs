using AgenteTickets.Models;
using AgenteTickets.Utils;
using log4net;
using RestSharp;
using System;
using System.Reflection;

namespace AgenteTickets.APISelfbilling.Services
{
    public static class AuthService
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static SelfbillingResponse GenerateToken(string apiUser, string apiPassword)
        {
            try
            {
                RestRequest request = new RestRequest()
                {
                    Resource = "api/auth",
                    Method = Method.Get,
                };

                _ = request.AddQueryParameter("usuario", apiUser);
                _ = request.AddQueryParameter("pass", apiPassword);

                RestResponse<SelfbillingResponse> response = SelfbillingHttp.Client.Execute<SelfbillingResponse>(request);

                if (response.Data?.Code != null)
                {
                    string message = $"API: {response.Data.Code}. {response.Data.Message}";
                    response.Data.Message = message;
                    log.Info(message);

                    if (response.Data.IsSuccessful)
                    {
                        response.Data.DecodedToken = JWT.Decode<SelfbillingJWTPayload>(response.Data.Token);
                        log.Info($"Fecha expiración token: {response.Data.DecodedToken.Expiration}");
                    }

                    return response.Data;
                }
                else if (response.ErrorException != null)
                {
                    throw response.ErrorException;
                }
                else
                {
                    throw new Exception($"Servidor: {(int)response.StatusCode}. {response.StatusDescription}");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new SelfbillingResponse
                {
                    Message = ex.GetBaseException().Message
                };
            }
        }

        public static SelfbillingResponse ValidateToken()
        {
            try
            {
                RestRequest request = new RestRequest()
                {
                    Resource = "api/auth",
                    Method = Method.Post,
                };

                RestResponse<SelfbillingResponse> response = SelfbillingHttp.Client.Execute<SelfbillingResponse>(request);

                if (response.Data?.Code != null)
                {
                    string message = $"API: {response.Data.Code}. {response.Data.Message}";
                    response.Data.Message = message;
                    log.Info(message);

                    if (response.Data.IsSuccessful)
                    {
                        response.Data.DecodedToken = JWT.Decode<SelfbillingJWTPayload>(response.Data.Token);
                    }

                    return response.Data;
                }
                else if (response.ErrorException != null)
                {
                    throw response.ErrorException;
                }
                else
                {
                    throw new Exception($"Servidor: {(int)response.StatusCode}. {response.StatusDescription}");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new SelfbillingResponse
                {
                    Message = ex.GetBaseException().Message
                };
            }
        }
    }
}
