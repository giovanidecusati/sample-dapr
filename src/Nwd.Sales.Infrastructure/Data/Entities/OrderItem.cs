using Newtonsoft.Json;

namespace Nwd.Sales.Infrastructure.Data.Entities
{
    internal class OrderItem
    {
        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [JsonProperty("productId")]
        public Guid ProductId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("gst")]
        public decimal GST { get; set; }
    }
}