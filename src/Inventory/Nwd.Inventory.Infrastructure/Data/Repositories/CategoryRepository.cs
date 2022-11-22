using Dapr.Client;
using Nwd.Inventory.Domain.Entities;
using Nwd.Inventory.Domain.Queries.GetSingleCategory;
using Nwd.Inventory.Domain.Repositories;

namespace Nwd.Inventory.Infrastructure.Data.Repositories
{
    public class CategoryRepository : DaprStateMgmtRepository<Category>, ICategoryRepository, ICategoryReadonlyRepository
    {
        public override string StoreName => "category";

        public override string StoreKeyName(Category entity) => $"{entity.Id}";

        public CategoryRepository(DaprClient daprClient) : base(daprClient)
        {
        }

        public async Task<GetSingleCategoryQueryResult> GetByIdAsync(GetSingleCategoryQuery getSingleCategoryQuery)
        {
            var result = new GetSingleCategoryQueryResult();
            var category = await GetByIdAsync(getSingleCategoryQuery.CategoryId);
            if (category != null)
            {
                result.Id = category.Id;
                result.Name = category.Name;
            }
            return result;
        }
    }
}
