using Newtonsoft.Json;

namespace Nwd.Sales.Infrastructure.Data.Entities
{
    public class Product
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
