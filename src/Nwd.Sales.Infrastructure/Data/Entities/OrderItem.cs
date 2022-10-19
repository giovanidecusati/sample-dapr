using Newtonsoft.Json;

namespace Nwd.Sales.Infrastructure.Data.Entities
{
    internal class OrderItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("orderId")]
        public Guid OrderId { get; private set; }

        [JsonProperty("productId")]
        public Guid ProductId { get; private set; }

        [JsonProperty("quantity")]
        public int Quantity { get; private set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; private set; }

        [JsonProperty("total")]
        public decimal Total { get; private set; }

        [JsonProperty("gst")]
        public decimal GST { get; private set; }
    }
}