namespace Nwd.Inventory.Domain.Queries.GetSingleCategory
{
    public interface ICategoryReadonlyRepository
    {
        Task<GetSingleCategoryQueryResult> GetByIdAsync(GetSingleCategoryQuery cetSingleCategoryQuery);
    }
}