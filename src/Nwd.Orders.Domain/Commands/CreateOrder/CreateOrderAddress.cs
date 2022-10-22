using System.ComponentModel.DataAnnotations;

namespace Nwd.Orders.Commands.CreateOrder
{
    public class CreateOrderAddress
    {
        [Required]
        public string State { get; set; } = null!;

        [Required]
        public string Region { get; set; } = null!;

        [Required]
        public string PostalCode { get; set; } = null!;

        [Required]
        public string AddressLine1 { get; set; } = null!;
    }
}