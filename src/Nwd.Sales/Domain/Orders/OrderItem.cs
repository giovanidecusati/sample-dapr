using Newtonsoft.Json;

namespace Nwd.Sales.Domain.Orders
{
    public class OrderItem
    {
        public OrderItem(OrderAgg order, Product product, int quantity)
        {
            _ = order ?? throw new ArgumentNullException(nameof(order));
            _ = product ?? throw new ArgumentNullException(nameof(product));

            Id = Guid.NewGuid();
            OrderId = order.Id;
            ProductId = product.Id;
            Quantity = quantity;
            UnitPrice = product.UnitPrice;
            Total = Quantity * UnitPrice;
            GST = Total * 0.10m;
        }

        [JsonProperty("id")]
        public Guid Id { get; private set; }

        [JsonProperty("orderId")]
        public Guid OrderId { get; private set; }

        [JsonProperty("productId")]
        public Guid ProductId { get; private set; }

        [JsonProperty("quantity")]
        public int Quantity { get; private set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; private set; }

        [JsonProperty("total")]
        public decimal Total { get; private set; }

        [JsonProperty("gst")]
        public decimal GST { get; private set; }
    }
}