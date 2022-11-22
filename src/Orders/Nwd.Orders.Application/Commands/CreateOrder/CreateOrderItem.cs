using System.ComponentModel.DataAnnotations;

namespace Nwd.Orders.Application.CreateOrder
{
    public class CreateOrderItem
    {
        [Required]
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}