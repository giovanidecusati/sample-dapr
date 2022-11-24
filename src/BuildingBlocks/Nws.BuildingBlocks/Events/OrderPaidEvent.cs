using MediatR;

namespace Nws.BuildingBlocks.Events
{
    public class OrderPaidEvent : INotification
    {
        public OrderPaidEvent() { }

        public OrderPaidEvent(string orderId)
        {
            OrderId = orderId;
        }

        public string OrderId { get; set; }
    }
}
