namespace Nwd.Orders.Domain.Entities
{
    public class OrderItem
    {
        public int Sequence { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }

        public decimal GST { get; set; }
    }
}