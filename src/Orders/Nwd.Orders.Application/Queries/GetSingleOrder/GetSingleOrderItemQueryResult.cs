namespace Nwd.Orders.Application.Queries.GetSingleOrder
{
    public class GetSingleOrderItemQueryResult
    {
        public int Sequence { get; set; }

        public GetSingleOrderProduct Product { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }

        public decimal GST { get; set; }
    }
}
