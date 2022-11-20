using System.ComponentModel.DataAnnotations;

namespace Nwd.Orders.Commands.CreateOrder
{
    public class CreateOrderItem
    {
        [Required]
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}