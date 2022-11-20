using Nwd.Orders.Domain.Entities;

namespace Nwd.Orders.Domain.Queries.ListOrder
{
    public class ListOrderQueryResult
    {
        public string OrderId { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

    }
}
