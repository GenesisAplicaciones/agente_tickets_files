using AgenteTickets.AppDataBase.Enums;
using System;

namespace AgenteTickets.AppDataBase.Models
{
    public class TicketLog
    {
        public virtual DateTime Id { get; set; }
        public virtual string TicketSerie { get; set; }
        public virtual string TicketFolio { get; set; }
        public virtual DateTime TicketDate { get; set; }
        public virtual decimal? TicketAmount { get; set; }
        public virtual string TicketStatusCode { get; set; }
        public virtual TypeMethod TypeMethod { get; set; }
        public virtual TypeAction TypeAction { get; set; }
        public virtual StatusAction StatusAction { get; set; }
        public virtual int? ApiResponseCode { get; set; }
        public virtual string Message { get; set; }
    }
}
