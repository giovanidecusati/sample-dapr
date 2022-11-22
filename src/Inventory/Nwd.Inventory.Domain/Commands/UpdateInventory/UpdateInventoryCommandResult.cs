namespace Nwd.Inventory.Domain.Commands.UpdateInventory
{
    public class UpdateInventoryCommandResult
    {
        public string ProductId { get; set; }
        public int StockLevel { get; set; }
    }
}