using MediatR;

namespace Nws.BuildingBlocks.Events
{
    public class OrderSubmittedEvent : INotification
    {
        public OrderSubmittedEvent(string orderId)
        {
            OrderId = orderId;
        }

        public string OrderId { get; set; }
    }
}