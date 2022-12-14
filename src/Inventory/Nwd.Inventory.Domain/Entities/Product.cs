namespace Nwd.Inventory.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public decimal UnitPrice { get; set; }
        public bool Enabled { get; set; }
    }
}
