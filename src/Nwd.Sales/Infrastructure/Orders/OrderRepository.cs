using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Orders;

namespace Nwd.Sales.Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly Container _container;

        public OrderRepository(Container container)
        {
            _container = container;
        }

        public async Task SaveAsync(OrderAgg entity)
        {
            await _container.CreateItemAsync(entity);
        }
    }
}
