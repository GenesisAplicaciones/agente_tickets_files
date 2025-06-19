using AgenteTickets.Models;
using log4net;
using RestSharp;
using System;
using System.Reflection;

namespace AgenteTickets.APISelfbilling.Services
{
    public class TicketService
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static SelfbillingResponse<TicketMetadata> UploadTicket(Ticket ticket)
        {
            try
            {
                SelfbillingHttp.RefreshToken();

                log.Info($"Cargar ticket. Serie: {ticket.Serie}, Folio: {ticket.Folio}, Fecha: {ticket.TicketDate}, Importe: {ticket.Amount}, Status: {ticket.StatusCode}");

                RestRequest request = new RestRequest()
                {
                    Resource = "api/tickets",
                    Method = Method.Post,
                    RequestFormat = DataFormat.Json,
                };

                _ = request.AddJsonBody(ticket);

                RestResponse<SelfbillingResponse<TicketMetadata>> response = SelfbillingHttp.Client.Execute<SelfbillingResponse<TicketMetadata>>(request);

                if (response.Data?.Code != null)
                {
                    string message = $"API: {response.Data.Code}. {response.Data.Message}";
                    response.Data.Message = message;
                    log.Info(message);

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

                return new SelfbillingResponse<TicketMetadata>
                {
                    Message = ex.GetBaseException().Message
                };
            }
        }

        public static SelfbillingResponse<TicketMetadata> CancelTicket(Ticket ticket)
        {
            try
            {
                SelfbillingHttp.RefreshToken();

                log.Info($"Cancelar ticket. Serie: {ticket.Serie}, Folio: {ticket.Folio}, Fecha: {ticket.TicketDate}, Importe: {ticket.Amount}, Status: {ticket.StatusCode}");

                RestRequest request = new RestRequest()
                {
                    Resource = "api/tickets/{serie}/{folio}",
                    Method = Method.Delete,
                    RequestFormat = DataFormat.Json,
                };

                _ = request.AddUrlSegment("serie", ticket.Serie);
                _ = request.AddUrlSegment("folio", ticket.Folio);

                _ = request.AddJsonBody(new CancellationReason
                {
                    User = "Agente tickets",
                    Message = "Cancelación desde POS."
                });

                RestResponse<SelfbillingResponse<TicketMetadata>> response = SelfbillingHttp.Client.Execute<SelfbillingResponse<TicketMetadata>>(request);

                if (response.Data?.Code != null)
                {
                    string message = $"API: {response.Data.Code}. {response.Data.Message}";
                    response.Data.Message = message;
                    log.Info(message);

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

                return new SelfbillingResponse<TicketMetadata>
                {
                    Message = ex.GetBaseException().Message
                };
            }
        }
    }
}
