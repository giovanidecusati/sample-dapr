using MediatR;
using Microsoft.Extensions.Logging;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Domain.Interfaces;

namespace Nwd.Orders.Application.Events
{
    public class ProductHandler : INotificationHandler<ProductCreatedEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductHandler> _logger;

        public ProductHandler(IProductRepository productRepository, ILogger<ProductHandler> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Handle(ProductCreatedEvent productCreatedEvent, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(productCreatedEvent.Id);
            if (product == null)
                await _productRepository.AddAsync(new Product()
                {
                    Id = productCreatedEvent.Id,
                    Name = productCreatedEvent.Name,
                    Category = productCreatedEvent.CategoryName,
                    UnitPrice = productCreatedEvent.UnitPrice
                });
        }
    }
}
