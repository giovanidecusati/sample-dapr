using Nwd.Inventory.Domain.Events;

namespace Nwd.Inventory.Domain.Entities
{
    public class Inventory
    {
        private List<InventoryUpdatedEvent> _inventoryEvents;

        public Inventory(string productId)
        {
            ProductId = productId;
            StockLevel = 0;
            _inventoryEvents = new List<InventoryUpdatedEvent>();
        }

        public string ProductId { get; private set; }

        public int StockLevel { get; private set; }

        public void AddItem(int quantity)
        {
            _inventoryEvents.Add(new InventoryUpdatedEvent(ProductId, StockLevel, quantity, TransactionType.AddItem));
            StockLevel += quantity;
        }

        public void RemoveItem(int quantity)
        {
            _inventoryEvents.Add(new InventoryUpdatedEvent(ProductId, StockLevel, quantity, TransactionType.RemoveItem));
            StockLevel -= quantity;
        }
    }
}
