using FluentValidation;
using Nwd.Sales.Commands.CreateOrder;
using Nwd.Sales.Domain.Orders;

namespace Nwd.Sales.Application.Commands.CreateOrder
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public readonly IOrderRepository _orderRepository;
        public readonly IProductRepository _productRepository;
        public readonly ICustomerRepository _customerRepository;

        public CreateOrderValidator(IOrderRepository orderRepository, IProductRepository productRepository, ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;

            RuleFor(x => x.Items).NotNull();

            RuleFor(x => x.CustomerId).NotNull();
            RuleFor(x => x.CustomerId).MustAsync(BeExistentCustomer);

            RuleFor(x => x.ShipTo).NotNull();
            RuleFor(x => x.ShipTo.AddressLine1).NotEmpty().When(x => x.ShipTo != null);
            RuleFor(x => x.ShipTo.State).NotEmpty().When(x => x.ShipTo != null);
            RuleFor(x => x.ShipTo.PostalCode).NotEmpty().When(x => x.ShipTo != null);
            RuleFor(x => x.ShipTo.Region).NotEmpty().When(x => x.ShipTo != null);
        }

        private async Task<bool> BeExistentCustomer(Guid customerId, CancellationToken cancellationToken = default)
        {
            return await _customerRepository.GetByIdAsync(customerId) != null;
        }
    }
}