using Dapr.Actors.Runtime;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using Nwd.Orders.Application.Commands.ProcessOrder;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Domain.Interfaces;

namespace Nwd.Orders.Application.Actors
{
    public class OrderProcessorActor : Actor, IOrderProcessorActor
    {
        private readonly ILogger<OrderProcessorActor> _logger;
        private readonly DaprClient _daprClient;
        private readonly IOrderRepository _orderRepository;

        public OrderProcessorActor(ActorHost host, ILogger<OrderProcessorActor> logger, DaprClient daprClient, IOrderRepository orderRepository) : base(host)
        {
            _logger = logger;
            _daprClient = daprClient;
            _orderRepository = orderRepository;
        }

        public async Task ProcessOrderAsync(string orderId)
        {
            _logger.LogInformation("Processing Order {orderId}", orderId);

            // Step 1 - Inventory - Reserve Items 
            // Step 2 - Payment - Charge Order
            // Step 3 - Payment - Charge Order
            var order = await _orderRepository.GetByIdAsync(orderId);

            order.Status = OrderStatus.Processing;

            await _orderRepository.UpdateAsync(order);

            _logger.LogInformation("Processing Order {orderId} has been completed.", orderId);
        }
    }
}
