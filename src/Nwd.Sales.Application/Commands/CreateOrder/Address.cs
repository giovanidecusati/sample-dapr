using System.ComponentModel.DataAnnotations;

namespace Nwd.Sales.Commands.CreateOrder
{
    public class Address
    {
        [Required]
        [StringLength(100)]
        public string State { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Region { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string PostalCode { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string AddressLine1 { get; set; } = null!;
    }
}