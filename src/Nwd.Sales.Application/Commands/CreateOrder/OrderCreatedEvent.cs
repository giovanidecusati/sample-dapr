using MediatR;

namespace Nwd.Sales.Application.Commands.CreateOrder
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
