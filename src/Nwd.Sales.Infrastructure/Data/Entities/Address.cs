namespace Nwd.Sales.Infrastructure.Data.Entities
{
    internal class Address
    {
        public string State { get; set; } = null!;

        public string Region { get; set; } = null!;

        public string PostalCode { get; set; } = null!;

        public string AddressLine1 { get; set; } = null!;
    }
}