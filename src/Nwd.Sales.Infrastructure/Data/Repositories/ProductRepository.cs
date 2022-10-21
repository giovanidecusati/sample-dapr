using AutoMapper;
using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Orders;
using Nwd.Sales.Infrastructure.Data.Interfaces;

namespace Nwd.Sales.Infrastructure.Data.Repositories
{
    internal class ProductRepository : CosmosDbRepository<Product, Entities.Product>, IProductRepository
    {        
        public override string ContainerName { get; } = "Products";

        public override string GenerateId(Product entity) => $"{entity.Id}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);

        public ProductRepository(ICosmosDbContainerFactory factory, IMapper mapper) : base(factory, mapper)
        {

        }
    }
}
