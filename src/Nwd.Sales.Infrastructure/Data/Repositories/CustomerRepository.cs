using AutoMapper;
using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Orders;
using Nwd.Sales.Infrastructure.Data.Interfaces;

namespace Nwd.Sales.Infrastructure.Data.Repositories
{
    public class CustomerRepository : CosmosDbRepository<Customer>, ICustomerRepository
    {
        private readonly IMapper _mapper;
        
        public override string ContainerName { get; } = "Customers";

        public override string GenerateId(Customer entity) => $"{entity.Id}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);

        public CustomerRepository(IMapper mapper, ICosmosDbContainerFactory factory) : base(factory)
        {
            _mapper = mapper;
        }     
    }
}
