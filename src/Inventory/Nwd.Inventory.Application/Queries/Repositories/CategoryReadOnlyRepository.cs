using Dapr.Client;
using Nwd.Inventory.Application.Queries.GetSingleCategory;

namespace Nwd.Inventory.Application.Queries.Repositories
{
    public class CategoryReadOnlyRepository : IGetSingleCategoryReadOnlyRepository
    {
        private readonly DaprClient _daprClient;

        public string StoreName => "category";

        public CategoryReadOnlyRepository(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task<GetSingleCategoryQueryResult> GetByIdAsync(GetSingleCategoryQuery getSingleCategoryQuery, CancellationToken cancellationToken = default)
        {
            return await _daprClient.GetStateAsync<GetSingleCategoryQueryResult>(StoreName, getSingleCategoryQuery.CategoryId, cancellationToken: cancellationToken);
        }
    }
}
