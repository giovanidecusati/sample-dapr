namespace Nwd.Inventory.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public static Transaction Increase(string inventoryId, int stockLevel, int quantity)
        {
            return new Transaction(inventoryId, stockLevel, TransactionType.AddItem, Math.Abs(quantity));
        }

        public static Transaction Decrease(string inventoryId, int stockLevel, int quantity)
        {
            return new Transaction(inventoryId, stockLevel, TransactionType.RemoveItem, Math.Abs(quantity));
        }

        private Transaction(string productId, int stockLevel, TransactionType transactionType, int quantity)
        {
            ProductId = productId;
            Date = DateTime.UtcNow;
            StockLevel = stockLevel;
            Quantity = quantity;
            TransactionType = transactionType;
            NewStockLevel = transactionType == TransactionType.AddItem ? stockLevel + quantity : stockLevel - quantity;
        }

        public string ProductId { get; private set; }
        public DateTime Date { get; private set; }
        public int StockLevel { get; private set; }
        public int NewStockLevel { get; private set; }
        public int Quantity { get; private set; }
        public TransactionType TransactionType { get; private set; }
    }
}