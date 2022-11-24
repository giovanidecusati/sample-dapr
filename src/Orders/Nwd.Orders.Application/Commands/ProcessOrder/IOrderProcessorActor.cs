using Dapr.Actors;
using Nwd.Orders.Domain.Entities;

namespace Nwd.Orders.Application.Commands.ProcessOrder
{
    public interface IOrderProcessorActor : IActor
    {
        Task ProcessOrderAsync(string orderId);
    }
}