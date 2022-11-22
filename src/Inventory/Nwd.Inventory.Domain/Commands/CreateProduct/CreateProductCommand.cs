using MediatR;

namespace Nwd.Inventory.Domain.Commands.CreateCategory
{
    public class CreateProductCommand : IRequest<CreateProductCommandResult>
    {
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
