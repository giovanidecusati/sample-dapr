using MediatR;
using Nwd.Inventory.Domain.Commands.CreateCategory;

namespace Nwd.Inventory.Api.Configuration
{
    public static class MediatRConfig
    {

        public static void SetupMediatR(this IServiceCollection services)
        {
            // Add Handlers
            services.AddMediatR(typeof(CategoryCommandHandler).Assembly);
        }
    }
}