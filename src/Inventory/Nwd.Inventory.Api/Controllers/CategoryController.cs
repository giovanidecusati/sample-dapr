using Microsoft.AspNetCore.Mvc;
using Nwd.Inventory.Domain.Commands.CreateCategory;

namespace Nwd.Inventory.Api.Controllers
{
    public class CategoryController : ApiControllerBase
    {
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(AcceptedResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> Post(CreateCategoryCommand createCategoryCommand)
        {
            var result = await Mediator.Send(createCategoryCommand);
            return CreatedAtRoute(new { id = result.CategoryId }, result);
        }
    }
}
