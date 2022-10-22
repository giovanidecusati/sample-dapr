using System.ComponentModel.DataAnnotations;

namespace Nwd.Orders.Commands.CreateOrder
{
    public class CreateOrderCustomer
    {
        [Required]
        public string Id { get; set; } = null!;
    }
}