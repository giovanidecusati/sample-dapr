using Nwd.Sales.Application.Services.Identity;
using System.Security.Claims;

namespace Nwd.Sales.WebApi.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?
                                .User?
                                .FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}
