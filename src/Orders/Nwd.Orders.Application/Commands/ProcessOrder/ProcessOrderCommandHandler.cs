using Dapr.Actors;
using Dapr.Actors.Client;
using MediatR;
using Microsoft.Extensions.Logging;
using Nwd.Orders.Application.Actors;
using Nwd.Orders.Domain.Interfaces;

namespace Nwd.Orders.Application.Commands.ProcessOrder
{
    public class ProcessOrderCommandHandler : IRequestHandler<ProcessOrderCommand, ProcessOrderCommandResult>
    {
        private readonly IActorProxyFactory _actorProxyFactory;
        private readonly ILogger<ProcessOrderCommandHandler> _logger;
        private readonly IOrderRepository _orderRepository;

        public ProcessOrderCommandHandler(IActorProxyFactory actorProxyFactory, ILogger<ProcessOrderCommandHandler> logger, IOrderRepository orderRepository)
        {
            _actorProxyFactory = actorProxyFactory;
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<ProcessOrderCommandResult> Handle(ProcessOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);

            var orderProcessorActor = _actorProxyFactory.CreateActorProxy<IOrderProcessorActor>(new ActorId(request.OrderId), nameof(OrderProcessorActor));

            await orderProcessorActor.ProcessOrderAsync(request.OrderId);

            return new ProcessOrderCommandResult(order.Id, order.Status.ToString(), order.Message);
        }
    }
}
