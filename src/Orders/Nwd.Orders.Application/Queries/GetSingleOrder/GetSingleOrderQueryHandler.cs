using MediatR;

namespace Nwd.Orders.Application.Queries.GetSingleOrder
{
    public class GetSingleOrderQueryHandler : IRequestHandler<GetSingleOrderQuery, GetSingleOrderQueryResult>
    {
        private readonly IOrderReadOnlyRepository _orderReadOnlyRepository;

        public GetSingleOrderQueryHandler(IOrderReadOnlyRepository orderReadOnlyRepository)
        {
            _orderReadOnlyRepository = orderReadOnlyRepository;
        }

        public async Task<GetSingleOrderQueryResult> Handle(GetSingleOrderQuery request, CancellationToken cancellationToken)
        {
            return await _orderReadOnlyRepository.GetSingleOrder(request);
        }
    }
}
