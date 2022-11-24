using MediatR;

namespace Nws.BuildingBlocks.Events
{
    public class OrderItemReservedEvent : INotification
    {
        public OrderItemReservedEvent() { }

        public OrderItemReservedEvent(string orderId)
        {
            OrderId = orderId;
        }

        public string OrderId { get; set; }
    }
}