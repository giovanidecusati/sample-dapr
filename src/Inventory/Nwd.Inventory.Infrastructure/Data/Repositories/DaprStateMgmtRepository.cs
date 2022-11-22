using Dapr.Client;
using Nwd.Inventory.Domain.Entities;
using Nwd.Inventory.Domain.Repositories;

namespace Nwd.Inventory.Infrastructure.Data.Repositories
{
    public abstract class DaprStateMgmtRepository<T> where T : BaseEntity
    {
        private IUnitOfWork _unitOfWork;
        private readonly DaprClient _client;
        public abstract string StoreName { get; }
        public abstract string StoreKeyName(T entity);

        public DaprStateMgmtRepository(DaprClient client)
        {
            _client = client;
        }

        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(T item, CancellationToken cancellationToken = default)
        {
            if (_unitOfWork == null)
                await _client.SaveStateAsync(StoreName, StoreKeyName(item), item, cancellationToken: cancellationToken);
            else
                EnlistAddOrUpdateTransaction(item);
        }

        public async Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _client.GetStateAsync<T>(StoreName, id, cancellationToken: cancellationToken);
        }

        private void EnlistAddOrUpdateTransaction(T item)
        {
            _unitOfWork.EnlistTransaction(StoreName, StoreKeyName(item), item);
        }
    }
}
