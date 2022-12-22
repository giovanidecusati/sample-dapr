using Dapr.Actors;

namespace Nwd.Inventory.Application.Actors
{
    public interface IInventoryProcessorActor : IActor
    {
        Task IncreaseInventoryAsync(string productId, int units);
        Task DecreaseInventoryAsync(string productId, int units);
    }
}