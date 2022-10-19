using AutoMapper;
using Newtonsoft.Json;

namespace Nwd.Sales.Infrastructure.Data.Entities
{
    internal class Product
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }
    }
}