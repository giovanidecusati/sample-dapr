using Newtonsoft.Json;

namespace Nwd.Orders.Domain.Entities
{
    public abstract class BaseEntity
    {
        [JsonProperty("id")]
        public virtual string Id { get; set; }
    }
}
