using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Nwd.Basket.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppInfoController : ControllerBase
    {
        [HttpGet]
        public dynamic Get()
        {
            return new
            {
                Version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? String.Empty,
                Name = Assembly.GetEntryAssembly()?.GetName().Name?.ToString() ?? String.Empty
            };
        }
    }
}
