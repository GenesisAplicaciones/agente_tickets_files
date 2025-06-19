using System.Collections.Generic;

namespace AgenteTickets.ClientData.AlohaXML.Models
{
    public class Item
    {
        public Item()
        {
            ChildItems = new List<Item>();
        }

        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemChitName2 { get; set; }
        public string ItemBOHName { get; set; }
        public decimal ItemQty { get; set; }
        public decimal ItemPrice { get; set; }
        public int ItemTaxID { get; set; }
        public decimal ItemUnitNetTotalCalculated { get; set; }
        public decimal ItemUnitTaxAmountCalculated { get; set; }
        public decimal ItemNetTotalCalculated { get; set; }
        public decimal ItemDiscountAmt { get; set; }
        public List<Item> ChildItems { get; set; }
    }
}
