using System.ComponentModel.DataAnnotations;

namespace Nwd.Orders.Commands.CreateOrder
{
    public class CreateOrderProduct
    {
        [Required]
        public string Id { get; set; } = null!;
    }
}