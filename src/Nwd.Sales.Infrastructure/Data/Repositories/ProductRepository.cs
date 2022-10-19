using AutoMapper;
using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Orders;
using Nwd.Sales.Infrastructure.Data.Interfaces;

namespace Nwd.Sales.Infrastructure.Data.Repositories
{
    public class ProductRepository : CosmosDbRepository<Product>, IProductRepository
    {
        private readonly IMapper _mapper;

        public override string ContainerName { get; } = "Products";

        public override string GenerateId(Product entity) => $"{entity.Id}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);

        public ProductRepository(IMapper mapper, ICosmosDbContainerFactory factory) : base(factory)
        {
            _mapper = mapper;
        }
    }
}
