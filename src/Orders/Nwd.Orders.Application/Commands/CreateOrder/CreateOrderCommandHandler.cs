using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Nwd.Orders.Application.CreateOrder;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Domain.Interfaces;

namespace Nwd.Orders.Domain.Commands.CreateOrder
{
    public class CreateOrderCommandHandler :
        IRequestHandler<CreateOrderCommand, CreateOrderCommandResult>
    {
        private readonly IValidator<CreateOrderCommand> _createOrderValidator;
        private readonly IOrderRepository _orderRepository;
        public readonly IProductRepository _productRepository;
        public readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(IValidator<CreateOrderCommand> createOrderValidator, IOrderRepository orderRepository, IProductRepository productRepository, ICustomerRepository customerRepository, ILogger<CreateOrderCommandHandler> logger)
        {
            _createOrderValidator = createOrderValidator;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task<CreateOrderCommandResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            await _createOrderValidator.ValidateAndThrowAsync(request);

            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            var order = new Order()
            {
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

            return new CreateOrderCommandResult(order.Id);
        }
    }
}

