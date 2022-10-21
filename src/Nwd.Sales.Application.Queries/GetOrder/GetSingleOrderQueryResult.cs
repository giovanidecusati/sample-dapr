namespace Nwd.Sales.Application.Queries.GetOrder
{
    public class GetSingleOrderQueryResult
    {
        public string Id { get; set; } = null!;

        public Guid CustomerId { get; set; }

        public List<GetSingleOrderItemQueryResult> Items { get; set; } = null!;

        public int Status { get; set; }

        public Address ShipTo { get; set; } = null!;
    }
}
