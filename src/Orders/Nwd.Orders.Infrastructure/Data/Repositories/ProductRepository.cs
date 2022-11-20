using Microsoft.Azure.Cosmos;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Domain.Interfaces;
using Nwd.Orders.Infrastructure.Data.Interfaces;

namespace Nwd.Orders.Infrastructure.Data.Repositories
{
    internal class ProductRepository : CosmosDbRepository<Product>, IProductRepository
    {
        public override string ContainerName { get; } = CosmosDbContainer.ProductsContainerName;

        public override string GenerateId(Product entity) => $"{entity.Id}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);

        public ProductRepository(ICosmosDbContainerFactory factory) : base(factory)
        {

        }
    }
}
