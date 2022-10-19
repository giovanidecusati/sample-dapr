using Newtonsoft.Json;

namespace Nwd.Sales.Infrastructure.Data.Entities
{
    internal class Customer
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
