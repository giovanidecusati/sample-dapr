using MediatR;

namespace Nwd.Sales.Application.Queries.GetOrder
{
    public class OrderQueryHandler : IRequestHandler<GetSingleOrderQuery, GetSingleOrderQueryResult>
    {
        private readonly IOrderReadOnlyRepository _orderReadOnlyRepository;

        public OrderQueryHandler(IOrderReadOnlyRepository orderReadOnlyRepository)
        {
            _orderReadOnlyRepository = orderReadOnlyRepository;
        }

        public async Task<GetSingleOrderQueryResult> Handle(GetSingleOrderQuery request, CancellationToken cancellationToken)
        {
            return await _orderReadOnlyRepository.GetSingleOrder(request);
        }
    }
}
