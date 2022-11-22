using MediatR;

namespace Nwd.Inventory.Domain.Commands.UpdateInventory
{
    public class DecreaseInventoryCommand : IRequest<UpdateInventoryCommandResult>
    {
        public string ProductId { get; set; }
        public int Units { get; set; }
    }
}
