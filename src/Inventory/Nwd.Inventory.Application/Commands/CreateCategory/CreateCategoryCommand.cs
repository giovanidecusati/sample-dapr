using MediatR;

namespace Nwd.Inventory.Application.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CreateCategoryCommandResult>
    {
        public string Name { get; set; }
    }
}
