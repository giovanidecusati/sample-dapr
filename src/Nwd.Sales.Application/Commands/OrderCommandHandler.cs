using FluentValidation;
using Microsoft.Extensions.Logging;
using Nwd.Sales.Application.Commands.CreateOrder;
using Nwd.Sales.Application.Queries.GetOrderStatus;
using Nwd.Sales.Commands.CreateOrder;
using Nwd.Sales.Domain.Orders;

namespace Nwd.Sales.Application.Commands
{
    public class OrderCommandHandler
    {
        private readonly IValidator<CreateOrderCommand> _createOrderValidator;
        private readonly OrderAggBuilder _orderAggBuilder;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderCommandHandler> _logger;

        public OrderCommandHandler(IValidator<CreateOrderCommand> createOrderValidation, OrderAggBuilder orderAggBuilder, IOrderRepository orderRepository, ILogger<OrderCommandHandler> logger)
        {
            _createOrderValidator = createOrderValidation;
            _orderAggBuilder = orderAggBuilder;
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<CreateOrderCommandResult> CreateOrder(CreateOrderCommand createOrderCommand)
        {
            _ = createOrderCommand ?? throw new ArgumentNullException(nameof(createOrderCommand));
            _logger.LogDebug("CreateOrder: {@createOrderCommand}", createOrderCommand);

            await _createOrderValidator.ValidateAndThrowAsync(createOrderCommand);
            _orderAggBuilder.WithShipTo(createOrderCommand.ShipTo.State, createOrderCommand.ShipTo.Region, createOrderCommand.ShipTo.PostalCode, createOrderCommand.ShipTo.AddressLine1);
            await _orderAggBuilder.WithCustomerAsync(createOrderCommand.CustomerId);
            foreach (var item in createOrderCommand.Items)
            {
                await _orderAggBuilder.WithItemAsync(item.ProductId, item.Quantity);
            }

            var order = _orderAggBuilder.Build();
            await _orderRepository.AddAsync(order);
            return new CreateOrderCommandResult(order.Id);
        }

        public async Task<GetOrderQueryResult> GetOrder(Guid orderId)
        {
            return await Task.FromResult(new GetOrderQueryResult());
        }
    }
}

