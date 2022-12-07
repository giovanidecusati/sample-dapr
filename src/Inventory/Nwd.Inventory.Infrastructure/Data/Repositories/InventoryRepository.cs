using Dapr.Client;
using Nwd.Inventory.Domain.Entities;
using Nwd.Inventory.Domain.Repositories;

namespace Nwd.Inventory.Infrastructure.Data.Repositories
{
    public class InventoryRepository : DaprRepositoryBase<Domain.Entities.Inventory>, IInventoryRepository
    {
        public override string StoreName => "nwd-inventory.inventory";

        public override string StoreKeyName(Domain.Entities.Inventory entity) => $"{entity.Id}";

        public InventoryRepository(DaprClient daprClient) : base(daprClient)
        {
        }
    }
}
