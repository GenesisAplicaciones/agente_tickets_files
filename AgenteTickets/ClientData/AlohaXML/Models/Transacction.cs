using System;
using System.Collections.Generic;

namespace AgenteTickets.ClientData.AlohaXML.Models
{
    public class Transacction
    {
        public Transacction()
        {
            Items = new List<Item>();
            Tenders = new List<Tender>();
        }

        public DateTime DOB { get; set; }
        public int Guests { get; set; }
        public int CheckNumber { get; set; }
        public string RevenueCenter { get; set; }
        public bool IsClose { get; set; }
        public decimal Total { get; set; }
        public decimal PaymentTip { get; set; }
        public string CounterFull { get; set; }
        public decimal Comp { get; set; }
        public List<Item> Items { get; set; }
        public List<Tender> Tenders { get; set; }
    }
}
