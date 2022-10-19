using System.ComponentModel.DataAnnotations;

namespace Nwd.Sales.Commands.CreateOrder
{
    public class OrderItem
    {
        [Required]
        public string ProductId { get; set; }

        [Required]
        [Range(0, 999)]
        public int Quantity { get; set; }
    }
}