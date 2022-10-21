using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Nwd.Sales.Application.Queries.GetOrder;
using Nwd.Sales.Commands.CreateOrder;
using Nwd.Sales.Domain.Orders;

namespace Nwd.Sales.Application.Commands.CreateOrder
{
    public class OrderCommandHandler :
        IRequestHandler<CreateOrderCommand, CreateOrderCommandResult>,
        INotificationHandler<OrderCreatedEvent>
    {
        private readonly IValidator<CreateOrderCommand> _createOrderValidator;
        private readonly OrderBuilder _orderAggBuilder;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IOrderReadOnlyRepository _orderReadOnlyRepository;

        public OrderCommandHandler(IValidator<CreateOrderCommand> createOrderValidation, OrderBuilder orderAggBuilder, IOrderRepository orderRepository, ILogger<OrderCommandHandler> logger, IMediator mediator, IOrderReadOnlyRepository orderReadOnlyRepository)
        {
            _createOrderValidator = createOrderValidation;
            _orderAggBuilder = orderAggBuilder;
            _orderRepository = orderRepository;
            _logger = logger;
            _mediator = mediator;
            _orderReadOnlyRepository = orderReadOnlyRepository;
        }

        public async Task<CreateOrderCommandResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            await _createOrderValidator.ValidateAndThrowAsync(request);
            _orderAggBuilder.WithShipTo(request.ShipTo.State, request.ShipTo.Region, request.ShipTo.PostalCode, request.ShipTo.AddressLine1);
            await _orderAggBuilder.WithCustomerAsync(request.CustomerId);
            foreach (var item in request.Items)
            {
                await _orderAggBuilder.WithItemAsync(item.ProductId, item.Quantity);
            }

            var order = _orderAggBuilder.Build();
            await _orderRepository.AddAsync(order);
            await _mediator.Publish(new OrderCreatedEvent(order.Id));
            return new CreateOrderCommandResult(order.Id);  
        }

        public async Task<GetSingleOrderQueryResult> GetOrder(GetSingleOrderQuery getSingleOrderQuery)
        {
            return await _orderReadOnlyRepository.GetSingleOrder(getSingleOrderQuery);
        }

        public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug("OrderCreatedEvent has been raised {@OrderCreatedEvent}", notification);
            await Task.CompletedTask;
        }
    }
}

