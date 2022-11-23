using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nwd.Orders.Infrastructure.Data.Interfaces;
using System.Diagnostics;

namespace Nwd.Orders.Api.Services
{
    public class HealthCheck : IHealthCheck
    {
        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;
        private readonly ILogger<HealthCheck> _logger;
        private static readonly string Component_CosmosDbName = "CosmosDB";
        private static readonly string Component_HealthyStatus = "Healthy";
        private static readonly string Component_DegradedStatus = "Degraded";
        private static readonly string Component_UnhealthyStatus = "Unhealthy";
        private static readonly string Message_Healthy = "Our application is healthy and in a normal, working state.";
        private static readonly string Message_Degraded = "Our application is still running, but not responding within an expected timeframe.";
        private static readonly string Message_Unhealthy = "Our application is unhealthy and is offline or an unhandled exception was thrown while executing the check.";
        private Dictionary<string, object> _components;

        public HealthCheck(ICosmosDbContainerFactory cosmosDbContainerFactory, ILogger<HealthCheck> logger)
        {
            _cosmosDbContainerFactory = cosmosDbContainerFactory;
            _logger = logger;
            _components = new Dictionary<string, object>(new[] {
                new KeyValuePair<string, object>(Component_CosmosDbName, Component_UnhealthyStatus)
            });
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            await CosmosDBHealthcheck(cancellationToken);

            if (_components.Values.Contains(Component_UnhealthyStatus))
                return await Task.FromResult(HealthCheckResult.Healthy(Message_Unhealthy, _components));
            else if (_components.Values.Contains(Component_DegradedStatus))
                return await Task.FromResult(HealthCheckResult.Healthy(Message_Degraded, _components));
            else
                return await Task.FromResult(HealthCheckResult.Healthy(Message_Healthy, _components));
        }

        private async Task CosmosDBHealthcheck(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Checking {Component_CosmosDbName} component health.");
            var helathStatus = Component_UnhealthyStatus;

            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                await _cosmosDbContainerFactory.CheckHealthAsync(cancellationToken);
                stopWatch.Stop();

                helathStatus = stopWatch.ElapsedMilliseconds < 300 ? Component_HealthyStatus : Message_Degraded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Component_CosmosDbName} is {helathStatus}.");
            }

            _components[Component_CosmosDbName] = helathStatus;
        }
    }
}
