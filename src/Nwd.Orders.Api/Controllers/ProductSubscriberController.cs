using Dapr;
using Microsoft.AspNetCore.Mvc;
using Nwd.Orders.Domain.Events;

namespace Nwd.Orders.Api.Controllers
{
    public class ProductSubscriberController : ApiControllerBase
    {
        private readonly ILogger<ProductSubscriberController> _logger;

        public ProductSubscriberController(ILogger<ProductSubscriberController> logger)
        {
            _logger = logger;
        }

        [Topic("queue-component", "new-product-topic")]
        [HttpPost("subscribe")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(AcceptedResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> Subscribe(ProductCreatedEvent productCreatedEvent)
        {
            _logger.LogInformation("ProductCreatedEvent {@productCreatedEvent}", productCreatedEvent);

            await Mediator.Publish(productCreatedEvent);
            return Accepted();
        }
    }
}
