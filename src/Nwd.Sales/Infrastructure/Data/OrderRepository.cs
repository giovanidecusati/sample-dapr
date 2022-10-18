using AutoMapper;
using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Orders;

namespace Nwd.Sales.Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly Container _container;
        private readonly IMapper _mapper;

        public OrderRepository(Container container, IMapper mapper)
        {
            _container = container;
            _mapper = mapper;
        }

        public async Task SaveAsync(OrderAgg entity)
        {
            await _container.CreateItemAsync(entity);
        }
    }
}
