using MediatR;

namespace Nwd.Sales.Application.Queries.GetOrder
{
    public class GetSingleOrderQuery : IRequest<GetSingleOrderQueryResult>
    {
        public string OrderId { get; set; } = null!;

        public string CustomerId { get; set; } = null!;
    }
}
