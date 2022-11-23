using Dapr.Actors.Runtime;
using Nwd.Orders.Application.Commands.ProcessOrder;
using Nwd.Orders.Domain.Entities;

namespace Nwd.Orders.Application.Actors
{
    public class OrderProcessorActor : Actor, IOrderProcessorActor
    {
        public OrderProcessorActor(ActorHost host) : base(host)
        {
        }

        public async Task ProcessOrderAsync(Order order)
        {
            await Task.CompletedTask;
        }
    }
}
