using MediatR;

namespace Nwd.Orders.Application.Queries.ListOrder
{
    public class ListOrderQueryHandler : IRequestHandler<ListOrderQuery, IList<ListOrderQueryResult>>
    {
        private readonly IListOrderReadOnlyRepository _orderReadOnlyRepository;

        public ListOrderQueryHandler(IListOrderReadOnlyRepository orderReadOnlyRepository)
        {
            _orderReadOnlyRepository = orderReadOnlyRepository;
        }

        public async Task<IList<ListOrderQueryResult>> Handle(ListOrderQuery request, CancellationToken cancellationToken)
        {
            return await _orderReadOnlyRepository.ListOrder(request);
        }
    }
}
