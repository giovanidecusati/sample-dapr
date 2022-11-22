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
        private bool disposedValue;

        public UnitOfWork(DaprClient client, ILogger<UnitOfWork> logger)
        {
            _requests = new Dictionary<string, List<StateTransactionRequest>>();
            _client = client;
            _logger = logger;
        }

        public async Task ExecuteStateTransactionAsync(CancellationToken cancellationToken = default)
        {
            foreach (var storeKey in _requests.Keys)
                await _client.ExecuteStateTransactionAsync(storeKey, _requests[storeKey]);
            _logger.LogDebug("ExecuteStateTransactionAsync has been completed.");
        }

        public void EnlistTransaction<T>(string storeKey, T item) where T : BaseEntity
        {
            _logger.LogDebug("Enlist transaction into {storeKey} with content {@item}");
            lock (_requests)
            {
                _requests.TryGetValue(storeKey, out var transactions);
                transactions = transactions ?? new List<StateTransactionRequest>();
                var binaryItem = JsonSerializer.SerializeToUtf8Bytes(item);
                transactions.Add(new StateTransactionRequest(storeKey, binaryItem, StateOperationType.Upsert));
                _requests.TryAdd(storeKey, transactions);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _client.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _requests.Clear();
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~UnitOfWork()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            _logger.LogDebug("Disposing unit of work.");
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
