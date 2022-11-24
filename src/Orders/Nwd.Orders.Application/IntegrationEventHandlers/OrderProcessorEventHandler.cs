using Dapr.Actors;
using Dapr.Actors.Client;
using MediatR;
using Nwd.Orders.Application.Actors;
using Nws.BuildingBlocks.Events;

namespace Nwd.Orders.Application.IntegrationEventHandlers
{
    public class OrderProcessorEventHandler :
        INotificationHandler<OrderSubmittedEvent>,
        INotificationHandler<OrderPaidEvent>
    {
        private readonly IActorProxyFactory _actorProxyFactory;

        public OrderProcessorEventHandler(IActorProxyFactory actorProxyFactory)
        {
            _actorProxyFactory = actorProxyFactory;
        }

        public async Task Handle(OrderSubmittedEvent notification, CancellationToken cancellationToken)
        {
            var orderProcessorActor = _actorProxyFactory.CreateActorProxy<IOrderProcessorActor>(new ActorId(notification.OrderId), nameof(OrderProcessorActor));

            await orderProcessorActor.UpdateInventoryAsync(notification.OrderId);
        }

        public async Task Handle(OrderPaidEvent notification, CancellationToken cancellationToken)
        {
            var orderProcessorActor = _actorProxyFactory.CreateActorProxy<IOrderProcessorActor>(new ActorId(notification.OrderId), nameof(OrderProcessorActor));

            await orderProcessorActor.ProcessPaymentAsync(notification.OrderId);
        }
    }
}
