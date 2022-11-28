﻿using Dapr.Client;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Domain.Interfaces;

namespace Nwd.Orders.Infrastructure.Data.Repositories
{
    internal class ProductRepository : DaprRepositoryBase<Product>, IProductRepository
    {
        public override string StoreName { get; } = nameof(Product);

        public override string StoreKey(Product entity) => $"{entity.Id}";

        public ProductRepository(DaprClient daprClient) : base(daprClient) { }

    }
}
