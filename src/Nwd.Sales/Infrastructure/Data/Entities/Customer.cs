using Newtonsoft.Json;

namespace Nwd.Sales.Infrastructure.Data.Entities
{
    public class Customer
    {
        [JsonProperty("id")]
        public Guid Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("email")]
        public string Email { get; private set; }
    }
}
