using MediatR;

namespace Nwd.Inventory.Domain.Queries.GetSingleCategory
{
    public class GetSingleCategoryQuery : IRequest<GetSingleCategoryQueryResult>
    {
        public string CategoryId { get; set; }
    }
}
