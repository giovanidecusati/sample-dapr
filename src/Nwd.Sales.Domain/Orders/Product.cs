namespace Nwd.Sales.Domain.Orders
{
    public class Product
    {
        private Product() { }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public decimal UnitPrice { get; private set; }
    }
}