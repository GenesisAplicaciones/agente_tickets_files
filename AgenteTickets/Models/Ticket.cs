using AgenteTickets.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AgenteTickets.Models
{
    public class Ticket
    {
        public Ticket()
        {
            Details = new List<TicketDetail>();
        }

        [JsonProperty("serie")]
        public string Serie { get; set; }

        [JsonProperty("folio")]
        public string Folio { get; set; }

        [JsonProperty("importe")]
        public decimal Amount { get; set; }

        [JsonProperty("descuento")]
        [DefaultValue(0)]
        public decimal? Discount { get; set; }

        [JsonProperty("fecha_ticket")]
        [JsonConverter(typeof(DateTimeFormatConverter), "yyyy-MM-dd")]
        public DateTime TicketDate { get; set; }

        [JsonProperty("fecha_entrega")]
        [DefaultValue("")]
        [JsonConverter(typeof(DateTimeFormatConverter), "yyyy-MM-dd")]
        public DateTime? DeliveryDate { get; set; }

        [JsonProperty("hora_entrega")]
        [JsonConverter(typeof(DateTimeFormatConverter), "HH:mm:ss")]
        public TimeSpan? DeliveryTime { get; set; }

        [JsonProperty("direccion_completa")]
        public string CustomerAddress { get; set; }

        [JsonProperty("nombre_cliente")]
        public string CustomerName { get; set; }

        [JsonProperty("telefono")]
        public string CustomerPhone { get; set; }

        [JsonProperty("forma_pago")]
        public object Payment { get; set; }

        [JsonProperty("mesa")]
        public string Table { get; set; }

        [JsonProperty("personas")]
        public int? Guests { get; set; }

        [JsonProperty("mesero")]
        public string ServerName { get; set; }

        [JsonProperty("propina")]
        public decimal? Tip { get; set; }

        [JsonProperty("nombre_propiedad")]
        public string StoreName { get; set; }

        [JsonProperty("canal_venta")]
        public string SalesChannel { get; set; }

        [JsonProperty("nombre_vendedor")]
        public string SellerName { get; set; }

        [JsonProperty("autorizacion_descuentos")]
        public string DiscountAuthorization { get; set; }

        [JsonProperty("nombre_cajero")]
        public string CashierName { get; set; }

        [JsonProperty("fecha_venta")]
        [JsonConverter(typeof(DateTimeFormatConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime? SaleDate { get; set; }

        [JsonProperty("checknum")]
        public string CheckNumber { get; set; }

        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("facturable")]
        public int? Billable { get; set; }

        [JsonProperty("detalles")]
        public List<TicketDetail> Details { get; set; }

        [JsonIgnore]
        public int? Consecutive { get; set; }

        [JsonIgnore]
        public bool? Canceled { get; set; }

        [JsonIgnore]
        public string StatusCode { get; set; }
    }
}
