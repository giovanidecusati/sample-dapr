using AutoMapper;

namespace Nwd.Sales.Infrastructure.Data.Entities
{
     internal class Address
    {
        public string State { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string AddressLine1 { get; set; }
    }
}