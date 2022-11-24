using MediatR;

namespace Nws.BuildingBlocks.Events
{
    public class ProductCreatedEvent : INotification
    {
        public ProductCreatedEvent() { }

        public ProductCreatedEvent(string id, string name, string categoryName, decimal unitPrice)
        {
            Id = id;
            Name = name;
            CategoryName = categoryName;
            UnitPrice = unitPrice;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
