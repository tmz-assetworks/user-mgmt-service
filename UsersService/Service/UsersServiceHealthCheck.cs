using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

namespace UsersService.Api.Service
{
    public class UsersServiceHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var isHealthy = true;
                if (isHealthy)
                {
                    Log.Information("A healthy result.");
                    return Task.FromResult(
                        HealthCheckResult.Healthy("A healthy result."));

                }
                else
                {
                    Log.Information("An unhealthy result.");
                    return Task.FromResult(
                    new HealthCheckResult(
                        context.Registration.FailureStatus, "An unhealthy result."));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unhealthy result by Exception.");
                return Task.FromResult(
                    new HealthCheckResult(
                        context.Registration.FailureStatus, "An unhealthy result."));
            }
        }
    }
}
