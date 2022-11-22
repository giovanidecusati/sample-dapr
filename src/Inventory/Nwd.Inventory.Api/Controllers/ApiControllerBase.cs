using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nwd.Inventory.Api.Filters;

namespace Nwd.Inventory.Api.Controllers
{
    [ApiController]
    [ApiExceptionFilter]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private IMediator _mediator = null!;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}
