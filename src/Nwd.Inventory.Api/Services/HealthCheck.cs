using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace Nwd.Inventory.Api.Services
{
    public class HealthCheck : IHealthCheck
    {
        private readonly ILogger<HealthCheck> _logger;
        private static readonly string Component_CosmosDbName = "CosmosDB";
        private static readonly string Component_HealthyStatus = "Healthy";
        private static readonly string Component_UnhealthyStatus = "Unhealthy";
        private static readonly string Message_Degraded = "Our application is still running, but not responding within an expected timeframe.";
        private static readonly string Message_Healthy = "Our application is healthy and in a normal, working state.";
        private static readonly string Message_Unhealthy = "Our application is unhealthy and is offline or an unhandled exception was thrown while executing the check.";
        private Dictionary<string, string> _components;

        public HealthCheck(ILogger<HealthCheck> logger)
        {
            _logger = logger;
            _components = new Dictionary<string, string>(new[] {
                new KeyValuePair<string, string>(Component_CosmosDbName, Component_UnhealthyStatus)
            });
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (_components.Values.Contains(Component_UnhealthyStatus))
                    return HealthCheckResult.Healthy(Message_Degraded);
                else
                    return await Task.FromResult(HealthCheckResult.Healthy(Message_Healthy));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Message_Unhealthy);
                return HealthCheckResult.Unhealthy(Message_Unhealthy, ex);

            }
        }

        private async Task CosmosDBHealthcheck(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Checking {Component_CosmosDbName} component health.");

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            // await _cosmosDbContainerFactory.CheckHealthAsync(cancellationToken);
            stopWatch.Stop();

            var helathStatus = stopWatch.ElapsedMilliseconds < 300 ? Component_HealthyStatus : Component_UnhealthyStatus;
            _components[Component_CosmosDbName] = helathStatus;

            _logger.LogInformation($"{Component_CosmosDbName} is {helathStatus}.");
        }
    }
}
