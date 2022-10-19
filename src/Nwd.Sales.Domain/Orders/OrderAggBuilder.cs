using FluentValidation;

namespace Nwd.Sales.Domain.Orders
{
    public class OrderAggBuilder
    {
        private readonly IValidator<OrderAgg> _orderAggValidator;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;

        private Address? _shipTo;
        private Customer? _customer;
        private OrderItemCollection _orderItemCollection;

        public OrderAggBuilder(IValidator<OrderAgg> orderAggValidator, IProductRepository productRepository, ICustomerRepository customerRepository)
        {
            _orderAggValidator = orderAggValidator;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _orderItemCollection = new OrderItemCollection();
        }

        public OrderAggBuilder WithShipTo(string state, string region, string postalCode, string addressLine1)
        {
            _shipTo = new Address(state, region, postalCode, addressLine1);
            return this;
        }

        public async Task<OrderAggBuilder> WithCustomerAsync(Guid customerId)
        {
            _customer = await _customerRepository.GetByIdAsync(customerId);
            return this;
        }

        public async Task<OrderAggBuilder> WithItemAsync(Guid producId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(producId);
            _orderItemCollection.Add(product, quantity);
            return this;
        }

        public OrderAgg Build()
        {
            _ = _customer ?? throw new ArgumentNullException(nameof(_customer));
            _ = _shipTo ?? throw new ArgumentNullException(nameof(_shipTo));

            return new OrderAgg(_orderAggValidator, _customer, _shipTo, _orderItemCollection);

        }
    }
}
