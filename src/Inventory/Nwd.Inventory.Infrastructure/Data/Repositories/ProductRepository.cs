using Dapr.Client;
using Nwd.Inventory.Domain.Entities;
using Nwd.Inventory.Domain.Repositories;

namespace Nwd.Inventory.Infrastructure.Data.Repositories
{
    public class ProductRepository : DaprRepositoryBase<Product>, IProductRepository
    {
        public override string StoreName => "product";

        public override string StoreKeyName(Product entity) => $"{entity.Id}";

        public ProductRepository(DaprClient daprClient) : base(daprClient)
        {
        }
    }
}
