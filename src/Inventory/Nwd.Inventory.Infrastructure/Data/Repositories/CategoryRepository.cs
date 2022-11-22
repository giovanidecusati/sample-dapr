﻿using Dapr.Client;
using Nwd.Inventory.Domain.Entities;
using Nwd.Inventory.Domain.Repositories;

namespace Nwd.Inventory.Infrastructure.Data.Repositories
{
    public class CategoryRepository : DaprStateMgmtRepository<Category>, ICategoryRepository
    {
        public override string StoreName => "category";

        public override string StoreKeyName(Category entity) => $"{entity.Id}";

        public CategoryRepository(DaprClient daprClient) : base(daprClient)
        {
        }
    }
}
