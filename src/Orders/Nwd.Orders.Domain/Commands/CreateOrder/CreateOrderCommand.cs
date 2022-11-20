using MediatR;
using Nwd.Orders.Domain.Commands.CreateOrder;
using System.ComponentModel.DataAnnotations;

namespace Nwd.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<CreateOrderCommandResult>
    {
        [Required]
        public List<CreateOrderItem> Items { get; set; } = null!;

        [Required]
        public string CustomerId { get; set; } = null!;

        [Required]
        public CreateOrderAddress ShipTo { get; set; } = null!;
    }
}
