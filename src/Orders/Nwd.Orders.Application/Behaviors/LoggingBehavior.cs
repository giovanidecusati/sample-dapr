using MediatR;
using Microsoft.Extensions.Logging;

namespace Nwd.Orders.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = request.GetType().Name;

            try
            {
                _logger.LogDebug("{requestName} {@request}", requestName, request);
            }
            catch (NotSupportedException)
            {
                _logger.LogDebug($"[Serialization ERROR] {requestName} Could not serialize the request.");
            }

            return await next();
        }
    }
}
