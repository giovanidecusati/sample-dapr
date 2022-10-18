using Newtonsoft.Json;

namespace Nwd.Sales.Domain.Orders
{
    public class OrderAgg
    {
        public OrderAgg(Guid customerId)
        {
            CustomerId = customerId;
            Id = Guid.NewGuid();
            Items = new List<OrderItem>();
            Status = OrderStatus.Processing;
        }

        [JsonProperty("id")]
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public List<OrderItem> Items { get; private set; }
        public OrderStatus Status { get; private set; }
        public Address ShipTo { get; private set; }

        public void AddItem(Product product, int quantity)
        {
            Items.Add(new OrderItem(product, quantity));
        }
    }
}