namespace Nwd.Inventory.Application.Queries.GetSingleCategory
{
    public interface IGetSingleCategoryReadOnlyRepository
    {
        Task<GetSingleCategoryQueryResult> GetByIdAsync(GetSingleCategoryQuery getSingleCategoryQuery, CancellationToken cancellationToken = default);
    }
}