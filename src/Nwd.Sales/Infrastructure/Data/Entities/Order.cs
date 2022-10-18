using Newtonsoft.Json;

namespace Nwd.Sales.Infrastructure.Data.Entities
{
    public class Order
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public Address ShipTo { get; set; } = null!;
    }
}
