using MediatR;

namespace Nwd.Inventory.Domain.Queries.GetSingleCategory
{
    public class GetSingleCategoryQueryHandler : IRequestHandler<GetSingleCategoryQuery, GetSingleCategoryQueryResult>
    {
        private readonly ICategoryReadonlyRepository _categoryReadonlyRepository;

        public GetSingleCategoryQueryHandler(ICategoryReadonlyRepository categoryReadonlyRepository)
        {
            _categoryReadonlyRepository = categoryReadonlyRepository;
        }

        public async Task<GetSingleCategoryQueryResult> Handle(GetSingleCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _categoryReadonlyRepository.GetByIdAsync(request);
        }
    }
}
