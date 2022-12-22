using Dapr.Actors.Runtime;
using MediatR;
using Microsoft.Extensions.Logging;
using Nwd.Inventory.Application.Commands.UpdateInventory;
using Nwd.Inventory.Domain;
using Nwd.Inventory.Domain.Repositories;

namespace Nwd.Inventory.Application.Actors
{
    public class InventoryProcessorActor : Actor, IInventoryProcessorActor
    {
        private readonly ILogger<InventoryProcessorActor> _logger;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMediator _mediator;

        public InventoryProcessorActor(ActorHost host, ILogger<InventoryProcessorActor> logger, IInventoryRepository inventoryRepository, IMediator mediator) : base(host)
        {
            _logger = logger;
            _inventoryRepository = inventoryRepository;
            _mediator = mediator;
        }

        public async Task IncreaseInventoryAsync(string productId, int units)
        {
            _logger.LogInformation("InventoryProcessorActor (IncreaseInventoryAsync) has been started for Product {productId}", productId);

            await StateManager.SetStateAsync("productId", productId);
            await StateManager.SetStateAsync("isCompleted", false);

            var inventory = await _inventoryRepository.GetByIdAsync(productId);

            inventory = inventory ?? new Domain.Entities.Inventory();
            inventory.StockLevel += units;
            inventory.Id = productId;

            await _inventoryRepository.AddAsync(inventory);

            await _mediator.Publish(new InventoryUpdatedEvent(inventory.Id, inventory.StockLevel, units, TransactionType.AddItem));

            await StateManager.SetStateAsync("isCompleted", true);

            _logger.LogInformation("InventoryProcessorActor (IncreaseInventoryAsync) has been completed for Product {productId}", productId);
        }

        public async Task DecreaseInventoryAsync(string productId, int units)
        {
            _logger.LogInformation("InventoryProcessorActor (DecreaseInventoryAsync) has been started for Product {productId}", productId);

            await StateManager.SetStateAsync("productId", productId);
            await StateManager.SetStateAsync("isCompleted", false);

            var inventory = await _inventoryRepository.GetByIdAsync(productId);

            inventory = inventory ?? new Domain.Entities.Inventory();
            inventory.StockLevel -= units;
            inventory.Id = productId;

            await _inventoryRepository.AddAsync(inventory);

            await _mediator.Publish(new InventoryUpdatedEvent(inventory.Id, inventory.StockLevel, units, TransactionType.RemoveItem));

            await StateManager.SetStateAsync("isCompleted", true);

            _logger.LogInformation("InventoryProcessorActor (DecreaseInventoryAsync) has been completed for Product {productId}", productId);
        }
    }
}
