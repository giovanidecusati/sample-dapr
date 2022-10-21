namespace Nwd.Sales.Infrastructure.Data.Entities
{
    internal class OrderItem
    {
        public string Id { get; set; } = null!;

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }

        public decimal GST { get; set; }
    }
}