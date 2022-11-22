using Nwd.Inventory.Domain.Entities;

namespace Nwd.Inventory.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task ExecuteStateTransactionAsync(CancellationToken cancellationToken = default);
        void EnlistTransaction<T>(string storeKey, T item) where T : BaseEntity;
    }
}
