namespace Nwd.Sales.Domain.Orders
{
    public class OrderItem
    {
        private OrderItem() { }

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

        public Guid Id { get; private set; }

        public Guid OrderId { get; private set; }

        public Guid ProductId { get; private set; }

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal Total { get; private set; }

        public decimal GST { get; private set; }
    }
}