using MediatR;

namespace Nwd.Orders.Application.Queries.ListOrder
{
    public class ListOrderQuery : IRequest<IList<ListOrderQueryResult>>
    {
        public string CustomerId { get; set; }
    }
}
