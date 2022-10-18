namespace Nwd.Sales.Domain.Orders
{
    public class OrderItem
    {
        public OrderItem(Product productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public Product ProductId { get; private set; }
        public int Quantity { get; private set; }
    }
}