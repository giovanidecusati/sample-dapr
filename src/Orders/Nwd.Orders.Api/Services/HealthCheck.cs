using Dapr.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace Nwd.Orders.Api.Services
{
    public class HealthCheck : IHealthCheck
    {
        private readonly ILogger<HealthCheck> _logger;
        private readonly DaprClient _daprClient;
        private static readonly string Component_Dapr = "Dapr";
        private static readonly string Component_HealthyStatus = "Healthy";
        private static readonly string Component_DegradedStatus = "Degraded";
        private static readonly string Component_UnhealthyStatus = "Unhealthy";
        private static readonly string Message_Healthy = "Our application is healthy and in a normal, working state.";
        private static readonly string Message_Degraded = "Our application is still running, but not responding within an expected timeframe.";
        private static readonly string Message_Unhealthy = "Our application is unhealthy and is offline or an unhandled exception was thrown while executing the check.";
        private Dictionary<string, object> _components;

        public HealthCheck(ILogger<HealthCheck> logger, DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
            _components = new Dictionary<string, object>(new[] { new KeyValuePair<string, object>(Component_Dapr, Component_UnhealthyStatus) });
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            await DaprHealthCheck(cancellationToken);

            if (_components.Values.Contains(Component_UnhealthyStatus))
                return await Task.FromResult(HealthCheckResult.Healthy(Message_Unhealthy, _components));
            else if (_components.Values.Contains(Component_DegradedStatus))
                return await Task.FromResult(HealthCheckResult.Healthy(Message_Degraded, _components));
            else
                return await Task.FromResult(HealthCheckResult.Healthy(Message_Healthy, _components));
        }

        private async Task DaprHealthCheck(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Checking {Component_Dapr} component health.");
            var helathStatus = Component_UnhealthyStatus;

            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var result = await _daprClient.CheckHealthAsync();
                stopWatch.Stop();

                if (result)
                    helathStatus = stopWatch.ElapsedMilliseconds < 300 ? Component_HealthyStatus : Message_Degraded;
                else
                    helathStatus = Component_UnhealthyStatus;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Component_Dapr} is {helathStatus}.");
            }

            _components[Component_Dapr] = helathStatus;
            _logger.LogInformation($"{Component_Dapr} is {helathStatus}.");
        }
    }
}
