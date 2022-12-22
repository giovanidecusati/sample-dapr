using Dapr.Actors.Runtime;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using Nwd.Orders.Application.Services.Inventory;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Domain.Interfaces;
using Nws.BuildingBlocks;
using Nws.BuildingBlocks.Events;

namespace Nwd.Orders.Application.Actors
{
    public class OrderProcessorActor : Actor, IOrderProcessorActor
    {
        private readonly ILogger<OrderProcessorActor> _logger;
        private readonly DaprClient _daprClient;
        private readonly IInventoryService _inventoryService;
        private readonly IOrderRepository _orderRepository;

        public OrderProcessorActor(ActorHost host, ILogger<OrderProcessorActor> logger, DaprClient daprClient, IOrderRepository orderRepository, IInventoryService inventoryService) : base(host)
        {
            _logger = logger;
            _daprClient = daprClient;
            _orderRepository = orderRepository;
            _inventoryService = inventoryService;
        }

        public async Task UpdateInventoryAsync(string orderId)
        {
            _logger.LogInformation("OrderProcessorActor (UpdateInventoryAsync) has been started for Order {orderId}", orderId);

            await StateManager.SetStateAsync("orderId", orderId);
            await StateManager.SetStateAsync("isUpdateInventoryCompleted", false);

            var order = await _orderRepository.GetByIdAsync(orderId);

            order.Status = OrderStatus.Processing;
            order.Messages.Add($"Changing status to Processing at {DateTime.Now}");

            foreach (var item in order.Items)
            {
                order.Messages.Add($"Reserving item {item.Product.Id} at {DateTime.Now}");
                try
                {
                    var result = _inventoryService.UpdateAsync(new UpdateInventoryModel() { ProductId = item.Product.Id, Units = item.Quantity }, string.Empty);
                }
                catch (Exception ex)
                {
                    order.Messages.Add($"Unable to reserve item {item.Product.Id} at {DateTime.Now}");
                    _logger.LogError(ex, $"Unable to reserve item {item.Product.Id} due to an error.");
                }
            }

            await _orderRepository.UpdateAsync(order);

            await _daprClient.PublishEventAsync(DaprConstants.DAPR_PUBSUB_NAME, nameof(OrderItemReservedEvent), new OrderItemReservedEvent(order.Id));

            await StateManager.SetStateAsync("isUpdateInventoryCompleted", true);

            _logger.LogInformation("OrderProcessorActor (UpdateInventoryAsync) has been completed for Order {orderId}", orderId);
        }

        public async Task ProcessPaymentAsync(string orderId)
        {
            _logger.LogInformation("OrderProcessorActor (ProcessPaymentAsync) has been started for Order {orderId}", orderId);

            await StateManager.SetStateAsync("orderId", orderId);
            await StateManager.SetStateAsync("isProcessPaymentCompleted", false);

            var order = await _orderRepository.GetByIdAsync(orderId);

            await _orderRepository.UpdateAsync(order);

            order.Status = OrderStatus.Completed;
            order.Messages.Add($"Changing status to Completed at {DateTime.Now}");
            await _orderRepository.UpdateAsync(order);

            await _daprClient.PublishEventAsync(DaprConstants.DAPR_PUBSUB_NAME, nameof(OrderPaidEvent), new OrderPaidEvent(order.Id));

            await StateManager.SetStateAsync("isProcessPaymentCompleted", true);

            _logger.LogInformation("OrderProcessorActor (ProcessPaymentAsync) has been completed for Order {orderId}", orderId);
        }
    }
}
