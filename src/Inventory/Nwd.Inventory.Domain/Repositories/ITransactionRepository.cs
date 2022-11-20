using Nwd.Inventory.Domain.Entities;

namespace Nwd.Inventory.Domain.Repositories
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetByIdAsync(string productId);
        Task AddAsync(Transaction item);
    }
}
