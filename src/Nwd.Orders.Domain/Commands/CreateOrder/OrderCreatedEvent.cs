using MediatR;

namespace Nwd.Orders.Domain.Commands.CreateOrder
{
    public class OrderCreatedEvent : INotification
    {
        public string OrderId { get; private set; }
        public OrderCreatedEvent(string orderId)
        {
            OrderId = orderId;
        }
    }
}
