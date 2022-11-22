using MediatR;

namespace Nwd.Inventory.Domain.Events
{
    public class InventoryUpdatedHandler : INotificationHandler<InventoryUpdatedEvent>
    {
        public async Task Handle(InventoryUpdatedEvent notification, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
