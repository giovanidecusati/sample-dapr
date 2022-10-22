using Newtonsoft.Json;

namespace Nwd.Orders.Domain.Entities
{
    public abstract class BaseEntity
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
