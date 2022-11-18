using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Nwd.Inventory.Domain.Events;

namespace Nwd.Inventory.Api.Controllers
{
    public class ProductController : ApiControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(AcceptedResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> Post()
        {
            using var client = new DaprClientBuilder().Build();

            var productCreatedEvent = new ProductCreatedEvent()
            {
                Category = "Monitor",
                Id = Guid.NewGuid().ToString(),
                Name = "Monitor Kogan",
                UnitPrice = 1.99m
            };

            // Publish an event/message using Dapr PubSub
            await client.PublishEventAsync("queue-component", "new-product-topic", productCreatedEvent);
            return Accepted();
        }
    }
}
