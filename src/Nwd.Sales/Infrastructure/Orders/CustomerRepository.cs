using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Orders;

namespace Nwd.Sales.Infrastructure.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly Container _container;

        public CustomerRepository(Container container)
        {
            _container = container;
        }

        public async Task<Customer> GetByIdAsync(Guid customerId)
        {
            try
            {
                var response = await _container.ReadItemAsync<Customer>(customerId.ToString(), new PartitionKey(customerId.ToString()));
                return response.Resource;

            }
            catch (CosmosException ex)
            {
                return null;
            }
        }
    }
}
