using Nwd.Sales.Domain.Common;

namespace Nwd.Sales.Domain.Orders
{
    public class Product : BaseEntity
    {
        public Product(string name, decimal unitPrice, string category)
        {
            Name = name;
            UnitPrice = unitPrice;
            Category = category;
        }        

        public string Name { get; private set; }

        public string Category { get; private set; }

        public decimal UnitPrice { get; private set; }
    }
}