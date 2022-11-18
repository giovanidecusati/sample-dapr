namespace Nwd.Inventory.Domain.Events
{
    public class ProductCreatedEvent
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
