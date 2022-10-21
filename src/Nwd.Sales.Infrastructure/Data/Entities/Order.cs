namespace Nwd.Sales.Infrastructure.Data.Entities
{
    internal class Order
    {
        public string Id { get; set; }

        public string CustomerId { get; set; }

        public List<OrderItem> Items { get; set; }

        public int Status { get; set; }

        public Address ShipTo { get; set; }

        public DateTime CreatedAt { get; private set; }
    }
}