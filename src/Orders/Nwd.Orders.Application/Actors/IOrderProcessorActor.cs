using Dapr.Actors;

namespace Nwd.Orders.Application.Actors
{
    public interface IOrderProcessorActor : IActor
    {
        Task UpdateInventoryAsync(string orderId);
        Task ProcessPaymentAsync(string orderId);
    }
}