using Dapr.Client;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Nwd.Orders.Application.CreateOrder;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Domain.Interfaces;
using Nws.BuildingBlocks;
using Nws.BuildingBlocks.Events;

namespace Nwd.Orders.Domain.Commands.CreateOrder
{
    public class CreateOrderCommandHandler :
        IRequestHandler<CreateOrderCommand, CreateOrderCommandResult>
    {
        private readonly IValidator<CreateOrderCommand> _createOrderValidator;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CreateOrderCommandHandler> _logger;
        private readonly DaprClient _daprClient;

        public CreateOrderCommandHandler(IValidator<CreateOrderCommand> createOrderValidator, IOrderRepository orderRepository, IProductRepository productRepository, ICustomerRepository customerRepository, ILogger<CreateOrderCommandHandler> logger, DaprClient daprClient)
        {
            _createOrderValidator = createOrderValidator;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _logger = logger;
            _daprClient = daprClient;
        }

        public async Task<CreateOrderCommandResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            await _createOrderValidator.ValidateAndThrowAsync(request);

            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            var order = new Order()
            {
                Status = OrderStatus.Submitted,
                Customer = customer,
                CreatedAt = DateTime.UtcNow,
                ShipTo = new Address()
                {
                    AddressLine1 = request.ShipTo.AddressLine1,
                    PostalCode = request.ShipTo.PostalCode,
                    Region = request.ShipTo.Region,
                    State = request.ShipTo.State
                }
            };

            var sequence = 0;
            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                order.Items.Add(new OrderItem()
                {
                    Sequence = sequence++,
                    Product = product,
                    Quantity = item.Quantity,
                    UnitPrice = product.UnitPrice,
                    GST = product.UnitPrice * 0.10m,
                    Total = item.Quantity * product.UnitPrice + 0.10m,
                });
            }

            await _orderRepository.AddAsync(order);

            // Publish an event/message using Dapr PubSub
            await _daprClient.PublishEventAsync(DaprConstants.DAPR_PUBSUB_NAME, nameof(OrderSubmittedEvent), new OrderSubmittedEvent(order.Id));

            return new CreateOrderCommandResult(order.Id);
        }
    }
}

