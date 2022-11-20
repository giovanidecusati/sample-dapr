using Nwd.Inventory.Domain.Events;

namespace Nwd.Inventory.Domain.Entities
{
    public class Transaction
    {
        public static Transaction AddItem(Inventory inventory, int quantity)
        {
            return new Transaction(inventory.ProductId, inventory.StockLevel, TransactionType.AddItem, Math.Abs(quantity));
        }

        public static Transaction RemoveItem(Inventory inventory, int quantity)
        {
            return new Transaction(inventory.ProductId, inventory.StockLevel, TransactionType.RemoveItem, Math.Abs(quantity));
        }

        private Transaction(string productId, int stockLevel, TransactionType transactionType, int quantity)
        {
            ProductId = productId;
            Date = DateTime.UtcNow;
            StockLevel = stockLevel;
            Quantity = quantity;
            TransactionType = transactionType;
            NewStockLevel = transactionType == TransactionType.AddItem ? stockLevel + stockLevel : stockLevel - stockLevel;
        }

        public string ProductId { get; private set; }
        public DateTime Date { get; private set; }
        public int StockLevel { get; private set; }
        public int NewStockLevel { get; private set; }
        public int Quantity { get; private set; }
        public TransactionType TransactionType { get; private set; }
    }
}