namespace Nwd.Inventory.Domain.Repositories
{
    public interface IInventoryRepository
    {
        Task<Entities.Inventory> GetByIdAsync(string productId);
        Task AddAsync(Entities.Inventory item);
    }
}
