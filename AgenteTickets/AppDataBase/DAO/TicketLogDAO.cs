using ACTools.NetFramework.Models;
using AgenteTickets.AppDataBase.Enums;
using AgenteTickets.AppDataBase.Models;
using AgenteTickets.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgenteTickets.AppDataBase.DAO
{
    public class TicketLogDAO : DAO<TicketLog>
    {
        public static bool Create<TMetadata>(Ticket ticket, SelfbillingResponse<TMetadata> response, TypeMethod typeMethod, TypeAction typeAction)
        {
            return Create(new TicketLog
            {
                Id = DateTime.Now,
                TicketSerie = ticket.Serie,
                TicketFolio = ticket.Folio,
                TicketDate = ticket.TicketDate,
                TicketAmount = ticket.Amount,
                TicketStatusCode = ticket.StatusCode,
                TypeMethod = typeMethod,
                TypeAction = typeAction,
                StatusAction = response.IsSuccessful ? StatusAction.COMPLETED : StatusAction.NOT_COMPLETED,
                ApiResponseCode = response.Code,
                Message = response.Message,
            });
        }

        public static bool Create(int? code, string message, TypeMethod typeMethod, TypeAction typeAction)
        {
            return Create(new TicketLog
            {
                Id = DateTime.Now,
                TypeMethod = typeMethod,
                TypeAction = typeAction,
                StatusAction = StatusAction.NOT_COMPLETED,
                ApiResponseCode = code,
                Message = message,
            });
        }

        public static List<SelectOption<string>> GetSeries()
        {
            using (ISession session = AppDBInstance.OpenSession())
            {
                return (from x in session.Query<TicketLog>()
                        group x by x.TicketSerie into g
                        select new SelectOption<string>
                        {
                            Text = g.Key,
                            Value = g.Key,
                        }).ToList();
            }
        }

        public static List<SelectOption<string>> GetTicketStatusCodes()
        {
            using (ISession session = AppDBInstance.OpenSession())
            {
                return (from x in session.Query<TicketLog>()
                        group x by x.TicketStatusCode into g
                        select new SelectOption<string>
                        {
                            Text = g.Key,
                            Value = g.Key,
                        }).ToList();
            }
        }

        public static List<TicketLog> List(TicketLog search)
        {
            using (ISession session = AppDBInstance.OpenSession())
            {
                DateTime date = search.Id ?? DateTime.Today;
                return (from x in session.Query<TicketLog>()
                        where (search.Id == null || (x.Id >= date && x.Id <= date.AddDays(1).AddSeconds(-1))) &&
                        (search.TicketSerie == "" || x.TicketSerie == search.TicketSerie) &&
                        (search.TicketFolio == "" || x.TicketFolio == search.TicketFolio) &&
                        (search.TicketDate == null || x.TicketDate == search.TicketDate) &&
                        (search.TicketAmount == null || x.TicketAmount == search.TicketAmount) &&
                        (search.TicketStatusCode == "" || x.TicketStatusCode == search.TicketStatusCode) &&
                        (search.TypeAction == TypeAction.NONE || x.TypeAction == search.TypeAction) &&
                        (search.TypeMethod == TypeMethod.NONE || x.TypeMethod == search.TypeMethod) &&
                        (search.StatusAction == StatusAction.NONE || x.StatusAction == search.StatusAction) &&
                        (search.ApiResponseCode == -1 || x.ApiResponseCode == search.ApiResponseCode) &&
                        x.Message.Contains(search.Message)
                        orderby x.Id descending
                        select x).Take(1000).ToList();
            }
        }

        public static List<TicketLog> List(DateTime ticketDate, string serie, TypeAction typeAction)
        {
            using (ISession session = AppDBInstance.OpenSession())
            {
                return (from x in session.Query<TicketLog>()
                        where x.TicketDate == ticketDate && x.TicketSerie == serie && x.TypeAction == typeAction
                        group x by new { x.TicketSerie, x.TicketFolio, x.TicketDate } into g
                        select new TicketLog
                        {
                            TicketSerie = g.Key.TicketSerie,
                            TicketFolio = g.Key.TicketFolio,
                            TicketDate = g.Key.TicketDate
                        }).ToList();
            }
        }

        public static DateTime? LastDate(TypeAction typeAction)
        {
            using (ISession session = AppDBInstance.OpenSession())
            {
                return (from x in session.Query<TicketLog>()
                        where x.TypeAction == typeAction
                        select x.TicketDate).Max();
            }
        }
    }
}
