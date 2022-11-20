namespace Nwd.Orders.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public decimal UnitPrice { get; set; }
    }
}