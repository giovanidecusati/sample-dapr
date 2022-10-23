using MediatR;

namespace Nwd.Orders.Domain.Events
{
    public class ProductCreatedEvent : INotification
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
