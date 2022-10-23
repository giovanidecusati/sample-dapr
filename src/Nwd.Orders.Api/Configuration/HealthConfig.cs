using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Nwd.Orders.Api.Configuration
{
    public static class HealthConfig
    {
        public static void MapAppHealthChecks(this IEndpointRouteBuilder app)
        {
            // MapHealthChecks
            var healthCheckOptions = new HealthCheckOptions();
            healthCheckOptions.ResponseWriter = async (c, r) =>
            {
                c.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(r);
                await c.Response.WriteAsync(result);
            };

            app.MapHealthChecks("/api/health", healthCheckOptions);
        }
    }
}
