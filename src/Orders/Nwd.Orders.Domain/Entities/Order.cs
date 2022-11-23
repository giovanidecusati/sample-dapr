namespace Nwd.Orders.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Customer Customer { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public OrderStatus Status { get; set; }

        public Address ShipTo { get; set; }
        public string Message { get; set; }
    }
}