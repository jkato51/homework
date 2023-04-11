using CorrelationId.Abstractions;

namespace Standard.Api.Middlewares;

public class LogResponseMiddleware
{
    private readonly ICorrelationContextAccessor _correlationContext;
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;

    public LogResponseMiddleware(RequestDelegate next, ILogger<LogResponseMiddleware> logger,
        ICorrelationContextAccessor correlationContext)
    {
        _next = next;
        _logger = logger;
        _correlationContext = correlationContext;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId = _correlationContext.CorrelationContext.CorrelationId;

        _logger.LogInformation($"StatusCode: {context.Response.StatusCode}. (CorrelationId: {correlationId})");

        await _next(context);
    }
}