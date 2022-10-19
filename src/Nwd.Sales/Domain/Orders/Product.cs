using Newtonsoft.Json;

namespace Nwd.Sales.Domain.Orders
{
    public class Product
    {
        [JsonProperty("id")]
        public Guid Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; private set; }
    }
}