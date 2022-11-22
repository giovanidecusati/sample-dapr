using MediatR;

namespace Nwd.Inventory.Application.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<CreateProductCommandResult>
    {
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
