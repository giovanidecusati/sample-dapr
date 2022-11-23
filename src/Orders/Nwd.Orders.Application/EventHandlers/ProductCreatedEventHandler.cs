using MediatR;
using Microsoft.Extensions.Logging;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Domain.Interfaces;
using Nws.BuildingBlocks.Events;

namespace Nwd.Orders.Application.EventHandlers
{
    public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductCreatedEventHandler> _logger;

        public ProductCreatedEventHandler(IProductRepository productRepository, ILogger<ProductCreatedEventHandler> logger)
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
