using Dapr.Client;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Domain.Interfaces;

namespace Nwd.Orders.Infrastructure.Data.Repositories
{
    internal class ProductRepository : DaprRepositoryBase<Product>, IProductRepository
    {
        public override string StoreName { get; } = "nwd-orders-product";

        public override string GenerateId(Product entity) => $"{entity.Id}";

        public ProductRepository(DaprClient daprClient) : base(daprClient) { }

    }
}
