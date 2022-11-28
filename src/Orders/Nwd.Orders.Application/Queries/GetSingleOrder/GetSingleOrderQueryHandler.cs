using MediatR;

namespace Nwd.Orders.Application.Queries.GetSingleOrder
{
    public class GetSingleOrderQueryHandler : IRequestHandler<GetSingleOrderQuery, GetSingleOrderQueryResult>
    {
        private readonly IGetSingleOrderReadOnlyRepository _orderReadOnlyRepository;

        public GetSingleOrderQueryHandler(IGetSingleOrderReadOnlyRepository orderReadOnlyRepository)
        {
            _orderReadOnlyRepository = orderReadOnlyRepository;
        }

        public async Task<GetSingleOrderQueryResult> Handle(GetSingleOrderQuery request, CancellationToken cancellationToken)
        {
            return await _orderReadOnlyRepository.GetSingleOrderAsync(request);
        }
    }
}
