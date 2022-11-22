using MediatR;
using Nwd.Inventory.Domain.Repositories;

namespace Nwd.Inventory.Application.Queries.GetSingleCategory
{
    public class GetSingleCategoryQueryHandler : IRequestHandler<GetSingleCategoryQuery, GetSingleCategoryQueryResult>
    {
        private readonly IGetSingleCategoryReadOnlyRepository _getSingleCategoryReadOnlyRepository;

        public GetSingleCategoryQueryHandler(IGetSingleCategoryReadOnlyRepository getSingleCategoryReadOnlyRepository)
        {
            _getSingleCategoryReadOnlyRepository = getSingleCategoryReadOnlyRepository;
        }

        public async Task<GetSingleCategoryQueryResult> Handle(GetSingleCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _getSingleCategoryReadOnlyRepository.GetByIdAsync(request);
        }
    }
}
