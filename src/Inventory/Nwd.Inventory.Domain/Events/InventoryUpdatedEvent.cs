using MediatR;

namespace Nwd.Inventory.Domain.Events
{
    public class InventoryUpdatedEvent : INotification
    {
        public InventoryUpdatedEvent(string productId, int stockLevel, int quantity, TransactionType transactionType)
        {
            ProductId = productId;
            StockLevel = stockLevel;
            Quantity = quantity;
            TransactionType = transactionType;
            Date = DateTime.UtcNow;
        }

        public string ProductId { get; private set; }
        public DateTime Date { get; private set; }

        public int StockLevel { get; private set; }

        public int Quantity { get; private set; }

        public TransactionType TransactionType { get; private set; }
    }
}
