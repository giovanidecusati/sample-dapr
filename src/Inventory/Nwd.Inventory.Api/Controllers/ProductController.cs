using Microsoft.AspNetCore.Mvc;
using Nwd.Inventory.Domain.Commands.CreateCategory;

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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateProductCommandResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> Post(CreateProductCommand createProductCommand)
        {
            var result = await Mediator.Send(createProductCommand);
            return CreatedAtRoute(new { id = result.ProductId }, result);
        }
    }
}
