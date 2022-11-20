using MediatR;

namespace Nwd.Orders.Domain.Queries.ListOrder
{
    public class ListOrderQuery : IRequest<IList<ListOrderQueryResult>>
    {
        public string CustomerId { get; set; }
    }
}
