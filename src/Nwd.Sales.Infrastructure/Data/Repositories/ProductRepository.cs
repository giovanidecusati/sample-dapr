using AutoMapper;
using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Orders;

namespace Nwd.Sales.Infrastructure.Data.Repositories
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
            try
            {
                var response = await _container.ReadItemAsync<Entities.Product>(productId.ToString(), new PartitionKey(productId.ToString()));
                return _mapper.Map<Product>(response.Resource);
            }
            catch (CosmosException ex)
            {
                return null;
            }
        }
    }
}
