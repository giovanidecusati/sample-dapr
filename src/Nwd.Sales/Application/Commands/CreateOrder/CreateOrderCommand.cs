using System.ComponentModel.DataAnnotations;

namespace Nwd.Sales.Commands.CreateOrder
{
    public class CreateOrderCommand
    {
        [Required]
        public List<OrderItem> Items = new List<OrderItem>();

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Address ShipTo = new Address();
    }
}
