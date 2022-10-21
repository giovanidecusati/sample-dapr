using FluentValidation;

namespace Nwd.Sales.Domain.Orders
{
    public class OrderBuilder
    {
        private readonly IValidator<Order> _orderAggValidator;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;

        private Address? _shipTo;
        private Customer? _customer;
        private OrderItemCollection _orderItemCollection;

        public OrderBuilder(IValidator<Order> orderAggValidator, IProductRepository productRepository, ICustomerRepository customerRepository)
        {
            _orderAggValidator = orderAggValidator;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _orderItemCollection = new OrderItemCollection();
        }

        public OrderBuilder WithShipTo(string state, string region, string postalCode, string addressLine1)
        {
            _shipTo = new Address(state, region, postalCode, addressLine1);
            return this;
        }

        public async Task<OrderBuilder> WithCustomerAsync(string customerId)
        {
            _customer = await _customerRepository.GetByIdAsync(customerId);
            return this;
        }

        public async Task<OrderBuilder> WithItemAsync(string producId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(producId);
            _orderItemCollection.Add(product, quantity);
            return this;
        }

        public Order Build()
        {
            _ = _customer ?? throw new ArgumentNullException(nameof(_customer));
            _ = _shipTo ?? throw new ArgumentNullException(nameof(_shipTo));

            return new Order(_orderAggValidator, _customer, _shipTo, _orderItemCollection);

        }
    }
}
