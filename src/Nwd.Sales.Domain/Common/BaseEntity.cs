namespace Nwd.Sales.Domain.Common
{
    public abstract class BaseEntity
    {
        public string Id { get; private set; }

        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
