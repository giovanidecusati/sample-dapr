using Dapr.Client;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Domain.Interfaces;

namespace Nwd.Orders.Infrastructure.Data.Repositories
{
    internal class OrderRepository : DaprRepositoryBase<Order>, IOrderRepository
    {
        public override string StoreName { get; } = "nwd-orders-order";

        public override string GenerateId(Order entity) => $"{Guid.NewGuid()}";

        public OrderRepository(DaprClient daprClient) : base(daprClient) { }
    }
}
