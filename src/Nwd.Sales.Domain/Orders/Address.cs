namespace Nwd.Sales.Domain.Orders
{
    public class Address
    {
        private Address() { }

        public Address(string state, string region, string postalCode, string addressLine1)
        {
            State = state;
            Region = region;
            PostalCode = postalCode;
            AddressLine1 = addressLine1;
        }

        public string State { get; private set; }

        public string Region { get; private set; }

        public string PostalCode { get; private set; }

        public string AddressLine1 { get; private set; }
    }
}