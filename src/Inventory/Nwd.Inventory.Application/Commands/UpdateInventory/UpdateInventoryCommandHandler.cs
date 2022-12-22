using Dapr.Actors;
using Dapr.Actors.Client;
using MediatR;
using Nwd.Inventory.Application.Actors;
using Nwd.Inventory.Domain.Repositories;

namespace Nwd.Inventory.Application.Commands.UpdateInventory
{
    public class UpdateInventoryCommandHandler :
        IRequestHandler<IncreaseInventoryCommand, UpdateInventoryCommandResult>,
        IRequestHandler<DecreaseInventoryCommand, UpdateInventoryCommandResult>
    {
        private readonly IActorProxyFactory _actorProxyFactory;
        private readonly IInventoryRepository _inventoryRepository;

        public UpdateInventoryCommandHandler(IActorProxyFactory actorProxyFactory, IInventoryRepository inventoryRepository)
        {
            _actorProxyFactory = actorProxyFactory;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<UpdateInventoryCommandResult> Handle(IncreaseInventoryCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            var inventoryProcessorActor = _actorProxyFactory.CreateActorProxy<IInventoryProcessorActor>(new ActorId(request.ProductId), nameof(InventoryProcessorActor));

            await inventoryProcessorActor.IncreaseInventoryAsync(request.ProductId, request.Units);

            var inventory = await _inventoryRepository.GetByIdAsync(request.ProductId);

            return new UpdateInventoryCommandResult() { ProductId = inventory.Id, StockLevel = inventory.StockLevel };
        }

        public async Task<UpdateInventoryCommandResult> Handle(DecreaseInventoryCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            var inventoryProcessorActor = _actorProxyFactory.CreateActorProxy<IInventoryProcessorActor>(new ActorId(request.ProductId), nameof(InventoryProcessorActor));

            await inventoryProcessorActor.DecreaseInventoryAsync(request.ProductId, request.Units);

            var inventory = await _inventoryRepository.GetByIdAsync(request.ProductId);

            return new UpdateInventoryCommandResult() { ProductId = inventory.Id, StockLevel = inventory.StockLevel };
        }
    }
}