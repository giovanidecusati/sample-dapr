﻿using MediatR;
using Nwd.Inventory.Domain.Entities;
using Nwd.Inventory.Domain.Events;
using Nwd.Inventory.Domain.Repositories;

namespace Nwd.Inventory.Domain.Commands.CreateCategory
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository, IMediator mediator, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mediator = mediator;
            _categoryRepository = categoryRepository;
        }

        public async Task<CreateProductCommandResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            var product = new Product()
            {
                CategoryId = request.CategoryId,
                Enabled = true,
                Name = request.Name,
                UnitPrice = request.UnitPrice
            };

            await _productRepository.AddAsync(product);

            var productCreatedEvent = new ProductCreatedEvent()
            {
                CategoryName = category.Name,
                Id = product.Id,
                Name = product.Name,
                UnitPrice = product.UnitPrice
            };

            await _mediator.Publish(productCreatedEvent);

            return new CreateProductCommandResult(product.Id);
        }
    }
}
