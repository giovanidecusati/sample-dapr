using System.ComponentModel.DataAnnotations;

namespace Nwd.Sales.Commands.CreateOrder
{
    public class CreateOrderCommand
    {
        [Required]
        public List<OrderItem> Items { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public Address ShipTo { get; set; }
}
}
