﻿using Dapr.Client;
using Nwd.Inventory.Domain.Entities;
using Nwd.Inventory.Domain.Repositories;

namespace Nwd.Inventory.Infrastructure.Data.Repositories
{
    public class TransactionRepository : DaprStateMgmtRepository<Transaction>, ITransactionRepository
    {
        public override string StoreName => "transaction";

        public override string StoreKeyName(Transaction entity) => $"{entity.Id}";

        public TransactionRepository(DaprClient daprClient) : base(daprClient)
        {
        }
    }
}
