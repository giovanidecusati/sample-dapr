using MediatR;

namespace Nwd.Orders.Application.Commands.ProcessOrder
{
    public class ProcessOrderCommand : IRequest<ProcessOrderCommandResult>
    {
        public string OrderId { get; set; }
    }
}
