using MediatR;
using Nwd.Inventory.Application.Commands.UpdateInventory;
using Nwd.Inventory.Domain.Entities;
using Nwd.Inventory.Domain.Repositories;

namespace Nwd.Inventory.Application.EventHandlers
{
    public class InventoryUpdatedHandler : INotificationHandler<InventoryUpdatedEvent>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InventoryUpdatedHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
            _transactionRepository.SetUnitOfWork(_unitOfWork);
        }

        public async Task Handle(InventoryUpdatedEvent notification, CancellationToken cancellationToken)
        {
            if (notification.TransactionType == Domain.TransactionType.AddItem)
                await _transactionRepository.AddAsync(Transaction.Increase(notification.ProductId, notification.StockLevel - notification.Quantity, notification.Quantity));
            else
                await _transactionRepository.AddAsync(Transaction.Decrease(notification.ProductId, notification.StockLevel + notification.Quantity, notification.Quantity));
        }
    }
}
