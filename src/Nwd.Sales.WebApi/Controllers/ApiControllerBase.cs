using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nwd.Sales.WebApi.Filters;

namespace Nwd.Sales.WebApi.Controllers
{
    [ApiController]
    [ApiExceptionFilter]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}
