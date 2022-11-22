using MediatR;

namespace Nwd.Inventory.Application.Commands.CreateProduct
{
    public class ProductCreatedEvent : INotification
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
