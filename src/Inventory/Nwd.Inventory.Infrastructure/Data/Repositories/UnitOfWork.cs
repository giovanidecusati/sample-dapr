using Dapr.Client;
using Microsoft.Extensions.Logging;
using Nwd.Inventory.Domain.Entities;
using Nwd.Inventory.Domain.Repositories;
using System.Text.Json;

namespace Nwd.Inventory.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ILogger<UnitOfWork> _logger;
        private readonly DaprClient _client;
        private Dictionary<string, List<StateTransactionRequest>> _requests;

        public UnitOfWork(DaprClient client, ILogger<UnitOfWork> logger)
        {
            _requests = new Dictionary<string, List<StateTransactionRequest>>();
            _client = client;
            _logger = logger;
        }

        public async Task ExecuteStateTransactionAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Executing ExecuteStateTransactionAsync {@requests}", _requests);
            foreach (var storeKey in _requests.Keys)
                await _client.ExecuteStateTransactionAsync(storeKey, _requests[storeKey], cancellationToken: cancellationToken);
            _logger.LogDebug("ExecuteStateTransactionAsync has been completed.");
        }

        public void EnlistTransaction<T>(string storeName, string storeKeyName, T item) where T : BaseEntity
        {
            _logger.LogDebug("Enlist transaction into {storeKey} with content {@item}", storeKeyName, item);
            _requests.TryGetValue(storeName, out var transactions);
            transactions = transactions ?? new List<StateTransactionRequest>();
            var binaryItem = JsonSerializer.SerializeToUtf8Bytes(item);
            transactions.Add(new StateTransactionRequest(storeKeyName, binaryItem, StateOperationType.Upsert));
            _requests.TryAdd(storeName, transactions);
        }

        public void Dispose()
        {
            ExecuteStateTransactionAsync().GetAwaiter().GetResult();
        }
    }
}
