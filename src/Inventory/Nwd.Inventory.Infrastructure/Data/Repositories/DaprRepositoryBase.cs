using Dapr.Client;
using Nwd.Inventory.Domain.Entities;

namespace Nwd.Inventory.Infrastructure.Data.Repositories
{
    public abstract class DaprRepositoryBase<T> where T : BaseEntity
    {
        private readonly DaprClient _client;
        public abstract string StoreName { get; }
        public abstract string StoreKeyName(T entity);

        public DaprRepositoryBase(DaprClient client)
        {
            _client = client;
        }

        public async Task AddAsync(T item, CancellationToken cancellationToken = default)
        {
            await _client.SaveStateAsync(StoreName, StoreKeyName(item), item, cancellationToken: cancellationToken);
        }

        public async Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _client.GetStateAsync<T>(StoreName, id, cancellationToken: cancellationToken);
        }
    }
}
