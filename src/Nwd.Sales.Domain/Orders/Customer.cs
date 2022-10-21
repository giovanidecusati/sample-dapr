using Nwd.Sales.Domain.Common;

namespace Nwd.Sales.Domain.Orders
{
    public class Customer : BaseEntity
    {
        public Customer(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; private set; }

        public string Email { get; private set; }
    }
}
