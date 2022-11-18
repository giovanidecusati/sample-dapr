using Nwd.Inventory.Domain.Events;

namespace Nwd.Inventory.Domain.Entities
{
    public class Product
    {
        public Product(string name, string categoryId, decimal unitPrice, bool enabled)
        {
            Name = name;
            CategoryId = categoryId;
            UnitPrice = unitPrice;
            Enabled = enabled;
        }

        public string Name { get; private set; }
        public string CategoryId { get; private set; }
        public decimal UnitPrice { get; private set; }
        public bool Enabled { get; private set; }
    }    
}
