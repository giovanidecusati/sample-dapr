using MediatR;
using Nwd.Inventory.Domain.Events;
using Nwd.Inventory.Domain.Repositories;

namespace Nwd.Inventory.Domain.Commands.UpdateInventory
{
    public class UpdateInventoryCommandHandler :
        IRequestHandler<IncreaseInventoryCommand, UpdateInventoryCommandResult>,
        IRequestHandler<DecreaseInventoryCommand, UpdateInventoryCommandResult>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateInventoryCommandHandler(IInventoryRepository inventoryRepository, IMediator mediator, IUnitOfWork unitOfWork)
        {
            _inventoryRepository = inventoryRepository;
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _inventoryRepository.SetUnitOfWork(_unitOfWork);
        }

        public async Task<UpdateInventoryCommandResult> Handle(IncreaseInventoryCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            var inventory = await _inventoryRepository.GetByIdAsync(request.ProductId);

            inventory = inventory ?? new Entities.Inventory();
            inventory.StockLevel += request.Units;
            inventory.Id = request.ProductId;

            await _inventoryRepository.AddAsync(inventory);

            await _mediator.Publish(new InventoryUpdatedEvent(inventory.Id, inventory.StockLevel, request.Units, TransactionType.AddItem));

            return new UpdateInventoryCommandResult() { ProductId = inventory.Id, StockLevel = inventory.StockLevel };
        }

        public async Task<UpdateInventoryCommandResult> Handle(DecreaseInventoryCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            var inventory = await _inventoryRepository.GetByIdAsync(request.ProductId);

            inventory = inventory ?? new Entities.Inventory();
            inventory.StockLevel -= request.Units;
            inventory.Id = request.ProductId;

            await _inventoryRepository.AddAsync(inventory);

            await _mediator.Publish(new InventoryUpdatedEvent(inventory.Id, inventory.StockLevel, request.Units, TransactionType.RemoveItem));

            return new UpdateInventoryCommandResult() { ProductId = inventory.Id, StockLevel = inventory.StockLevel };
        }
    }
}