namespace Nwd.Sales.Infrastructure.Data.Entities
{
    internal class Product
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public decimal Category { get; set; }

        public decimal UnitPrice { get; set; }
    }
}