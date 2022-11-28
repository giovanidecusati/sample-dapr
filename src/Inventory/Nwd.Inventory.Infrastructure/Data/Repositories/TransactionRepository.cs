using Dapr.Client;
using Nwd.Inventory.Domain.Entities;
using Nwd.Inventory.Domain.Repositories;

namespace Nwd.Inventory.Infrastructure.Data.Repositories
{
    public class TransactionRepository : DaprRepositoryBase<Transaction>, ITransactionRepository
    {
        public override string StoreName => nameof(Transaction);

        public override string StoreKeyName(Transaction entity) => $"{entity.Id}";

        public TransactionRepository(DaprClient daprClient) : base(daprClient)
        {
        }
    }
}
