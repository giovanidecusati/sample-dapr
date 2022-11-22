using MediatR;

namespace Nwd.Inventory.Application.Queries.GetSingleCategory
{
    public class GetSingleCategoryQuery : IRequest<GetSingleCategoryQueryResult>
    {
        public string CategoryId { get; set; }
    }
}
