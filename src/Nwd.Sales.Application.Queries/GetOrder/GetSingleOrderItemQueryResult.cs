namespace Nwd.Sales.Application.Queries.GetOrder
{
    public class GetSingleOrderItemQueryResult
    {
        public string Id { get; set; } = null!;

        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }

        public decimal GST { get; set; }
    }
}
