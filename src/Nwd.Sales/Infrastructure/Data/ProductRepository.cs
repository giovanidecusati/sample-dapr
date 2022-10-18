using AutoMapper;
using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Orders;

namespace Nwd.Sales.Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly Container _container;
        private readonly IMapper _mapper;

        public ProductRepository(Container container, IMapper mapper)
        {
            _container = container;
            _mapper = mapper;
        }

        public async Task<Product> GetByIdAsync(Guid productId)
        {
            var customer = await _container.ReadItemAsync<Entities.Product>(productId.ToString(), new PartitionKey("id"));
            return _mapper.Map<Product>(customer);
        }
    }
}
