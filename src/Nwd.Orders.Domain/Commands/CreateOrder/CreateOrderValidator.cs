using FluentValidation;
using Nwd.Orders.Commands.CreateOrder;
using Nwd.Orders.Domain.Interfaces;

namespace Nwd.Orders.Domain.Commands.CreateOrder
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
            RuleForEach(x => x.Items).ChildRules(x =>
            {
                x.RuleFor(item => item.Product).NotNull();
                x.RuleFor(item => item.Product.Id).MustAsync(BeExistentProduct);

                x.RuleFor(item => item.Quantity).GreaterThan(0);
            });

            RuleFor(x => x.Customer).NotNull();
            RuleFor(x => x.Customer.Id).MustAsync(BeExistentCustomer);

            RuleFor(x => x.ShipTo).NotNull();
            RuleFor(x => x.ShipTo.AddressLine1).NotEmpty().When(x => x.ShipTo != null);
            RuleFor(x => x.ShipTo.State).NotEmpty().When(x => x.ShipTo != null);
            RuleFor(x => x.ShipTo.PostalCode).NotEmpty().When(x => x.ShipTo != null);
            RuleFor(x => x.ShipTo.Region).NotEmpty().When(x => x.ShipTo != null);
        }

        private async Task<bool> BeExistentCustomer(string customerId, CancellationToken cancellationToken = default)
        {
            return await _customerRepository.GetByIdAsync(customerId) != null;
        }

        private async Task<bool> BeExistentProduct(string productId, CancellationToken cancellationToken = default)
        {
            return await _productRepository.GetByIdAsync(productId) != null;
        }
    }
}