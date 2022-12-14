using MediatR;

namespace Nwd.Orders.Application.Queries.GetSingleOrder
{
    public class GetSingleOrderQuery : IRequest<GetSingleOrderQueryResult>
    {
        public string OrderId { get; set; } = null!;

        public string CustomerId { get; set; } = null!;
    }
}
