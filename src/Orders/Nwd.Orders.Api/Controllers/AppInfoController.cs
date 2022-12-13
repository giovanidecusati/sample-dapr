using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Nwd.Orders.Api.Controllers
{
    [Route("api/[controller]")]
    public class AppInfoController : ControllerBase
    {        
        [HttpGet]
        public dynamic Get()
        {
            return new
            {
                Version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0"
            };
        }
    }
}
