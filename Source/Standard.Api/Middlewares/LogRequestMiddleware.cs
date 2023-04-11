using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Http.Extensions;

namespace Standard.Api.Middlewares;

public class LogRequestMiddleware
{
    private readonly ICorrelationContextAccessor _correlationContext;
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;

    public LogRequestMiddleware(RequestDelegate next, ILogger<LogRequestMiddleware> logger,
        ICorrelationContextAccessor correlationContext)
    {
        _next = next;
        _logger = logger;
        _correlationContext = correlationContext;
    }

    public async Task Invoke(HttpContext context)
    {
        var url = context.Request.GetDisplayUrl();

        var correlationId = _correlationContext.CorrelationContext.CorrelationId;

        _logger.LogInformation(
            $"Scheme: {context.Request.Scheme}, " +
            $"Host: {context.Request.Host}, " +
            $"Path: {context.Request.Path}, " +
            $"Method: {context.Request.Method}, " +
            $"Url: {url}, " +
            $"CorrelationId: {correlationId}");

        await _next(context);
    }
}