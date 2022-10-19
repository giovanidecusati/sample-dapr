using Newtonsoft.Json;

namespace Nwd.Sales.Domain.Common
{
    public abstract class BaseEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
