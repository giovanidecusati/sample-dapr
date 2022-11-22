using Microsoft.AspNetCore.Mvc;
using Nwd.Inventory.Application.Commands.CreateCategory;
using Nwd.Inventory.Application.Queries.GetSingleCategory;

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


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetSingleCategoryQueryResult))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedObjectResult))]
        public async Task<GetSingleCategoryQueryResult> Get([FromQuery] GetSingleCategoryQuery getSingleCategoryQuery)
        {
            return await Mediator.Send(getSingleCategoryQuery);
        }
    }
}