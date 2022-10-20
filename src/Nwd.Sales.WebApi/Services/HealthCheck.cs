using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nwd.Sales.Infrastructure.Data.Interfaces;

namespace Nwd.Sales.WebApi.Services
{
    public class HealthCheck : IHealthCheck
    {
        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;
        private readonly ILogger<HealthCheck> _logger;
        private static readonly string Component_CosmosDbName = "CosmosDB";
        private static readonly string Component_HealthyStatus = "Healthy";
        private static readonly string Component_UnhealthyStatus = "Unhealthy";
        private static readonly string Message_Degraded = "Our application is still running, but not responding within an expected timeframe.";
        private static readonly string Message_Healthy = "Our application is healthy and in a normal, working state.";
        private static readonly string Message_Unhealthy = "Our application is unhealthy and is offline or an unhandled exception was thrown while executing the check.";

        public HealthCheck(ICosmosDbContainerFactory cosmosDbContainerFactory, ILogger<HealthCheck> logger)
        {
            _cosmosDbContainerFactory = cosmosDbContainerFactory;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var components = new Dictionary<string, object>();
            components.Add(Component_CosmosDbName, Component_UnhealthyStatus);

            try
            {
                await _cosmosDbContainerFactory.CheckHealthAsync(cancellationToken);
                components[Component_CosmosDbName] = Component_HealthyStatus;

                return HealthCheckResult.Healthy(Message_Healthy, components);
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(Message_Unhealthy, ex, components);

            }
        }
    }
}
