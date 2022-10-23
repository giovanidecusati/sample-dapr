using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Nwd.Orders.Domain.Events;

namespace Nwd.Orders.Api.Controllers
{
    public class ProductController : ApiControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [Topic("queue-service", "new-product-topic")]
        [HttpPost("subscribe")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(AcceptedResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> Subscribe(ProductCreatedEvent productCreatedEvent)
        {
            _logger.LogInformation("ProductCreatedEvent {@productCreatedEvent}", productCreatedEvent);

            await Mediator.Publish((ProductCreatedEvent)productCreatedEvent);
            return Accepted();
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
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
            await client.PublishEventAsync("queue-service", "new-product-topic", productCreatedEvent);
            return Accepted();
        }
    }
}
