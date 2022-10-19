using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Orders;

namespace Nwd.Sales.Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly Container _container;

        public ProductRepository(Container container)
        {
            _container = container;
        }

        public async Task<Product> GetByIdAsync(Guid productId)
        {
            try
            {
                var response = await _container.ReadItemAsync<Product>(productId.ToString(), new PartitionKey(productId.ToString()));
                return response.Resource;

            }
            catch (CosmosException ex)
            {
                return null;
            }
        }
    }
}
