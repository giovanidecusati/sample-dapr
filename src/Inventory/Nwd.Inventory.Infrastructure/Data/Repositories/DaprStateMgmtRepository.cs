using Dapr.Client;
using Nwd.Inventory.Domain.Entities;

namespace Nwd.Inventory.Infrastructure.Data.Repositories
{
    public abstract class DaprStateMgmtRepository<T> where T : BaseEntity
    {
        private readonly DaprClient _client;
        public abstract string StoreName { get; }
        public abstract string StateKeyName { get; }

        public DaprStateMgmtRepository(DaprClient client)
        {
            _client = client;
        }

        public async Task AddAsync(T item, CancellationToken cancellationToken = default)
        {
            await _client.SaveStateAsync(StoreName, StateKeyName, item, cancellationToken: cancellationToken);
        }

        public async Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _client.GetStateAsync<T>(StoreName, id, cancellationToken: cancellationToken);
        }
    }
}
