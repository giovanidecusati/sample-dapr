using Newtonsoft.Json;

namespace Nwd.Sales.Infrastructure.Data.Entities
{
    internal class Product
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public decimal Category { get; set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }
    }
}