using Newtonsoft.Json;

namespace AgenteTickets.Models
{
    public class TicketDetail
    {
        public TicketDetail()
        {
            Taxes = new int[4] { 1, 0, 0, 0 };
        }

        [JsonIgnore]
        public string DetailId { get; set; }

        [JsonProperty("no_identificacion")]
        public string ProductId { get; set; }

        [JsonProperty("c_prod_serv")]
        public string ProductOrServiceKey { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("cantidad")]
        public int? Quantity { get; set; }

        [JsonProperty("c_unidad")]
        public string UnitKey { get; set; }

        [JsonProperty("unidad")]
        public string UnitName { get; set; }

        [JsonProperty("descripcion")]
        public string Description { get; set; }

        [JsonProperty("valor_unitario")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("importe")]
        public decimal Amount { get; set; }

        [JsonProperty("descuento")]
        public decimal? Discount { get; set; }

        [JsonProperty("categoria_producto")]
        public string ProductCategory { get; set; }

        [JsonProperty("categoria_producto_nivel2")]
        public string ProductCategoryLevel2 { get; set; }

        [JsonProperty("nivel_descuento")]
        public string DiscountLevel { get; set; }

        [JsonProperty("tipo_descuento")]
        public string DiscountType { get; set; }

        [JsonProperty("impuestos")]
        public int[] Taxes { get; set; }

        [JsonIgnore]
        public decimal DiscountAmount { get; set; }
    }
}
