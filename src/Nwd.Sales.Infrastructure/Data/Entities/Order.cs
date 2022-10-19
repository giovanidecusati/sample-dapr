using Newtonsoft.Json;

namespace Nwd.Sales.Infrastructure.Data.Entities
{
    internal class Order
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("customerId")]
        public Guid CustomerId { get; set; }

        [JsonProperty("items")]
        public List<OrderItem> Items { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("shipTo")]
        public Address ShipTo { get; set; }
    }
}