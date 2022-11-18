using MediatR;

namespace Nwd.Inventory.Domain.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CreateCategoryCommandResult>
    {
        public string Name { get; set; }
    }
}
