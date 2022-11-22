using MediatR;
using Nwd.Inventory.Application.Queries.GetSingleCategory;

namespace Nwd.Inventory.Application.Queries
{
    public class CategoryQueryHandler : IRequestHandler<GetSingleCategoryQuery, GetSingleCategoryQueryResult>
    {
        private readonly IGetSingleCategoryReadOnlyRepository _getSingleCategoryReadOnlyRepository;

        public CategoryQueryHandler(IGetSingleCategoryReadOnlyRepository getSingleCategoryReadOnlyRepository)
        {
            _getSingleCategoryReadOnlyRepository = getSingleCategoryReadOnlyRepository;
        }

        public async Task<GetSingleCategoryQueryResult> Handle(GetSingleCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _getSingleCategoryReadOnlyRepository.GetByIdAsync(request);
        }
    }
}
