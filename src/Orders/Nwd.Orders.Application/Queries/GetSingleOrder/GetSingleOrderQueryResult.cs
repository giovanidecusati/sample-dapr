using Nwd.Orders.Domain.Entities;

namespace Nwd.Orders.Application.Queries.GetSingleOrder
{
    public class GetSingleOrderQueryResult
    {
        public string Id { get; set; }

        public GetSingleOrderCustomer Customer { get; set; }

        public List<GetSingleOrderItemQueryResult> Items { get; set; }

        public OrderStatus Status { get; set; }

        public GetSingleOrderAddress ShipTo { get; set; }
    }
}
