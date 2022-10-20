using MediatR;
using Nwd.Sales.Application.Commands.CreateOrder;
using System.ComponentModel.DataAnnotations;

namespace Nwd.Sales.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<CreateOrderCommandResult>
    {
        [Required]
        public List<OrderItem> Items { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public Address ShipTo { get; set; }
    }
}
