using Nwd.Inventory.Domain.Entities;

namespace Nwd.Inventory.Domain.Repositories
{
    public interface IInventoryRepository : IAggRootRepository<Entities.Inventory>
    {
        void EnlistAddOrUpdateTransaction(Entities.Inventory item);
    }
}
