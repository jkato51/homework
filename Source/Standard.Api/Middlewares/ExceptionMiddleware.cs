﻿using CorrelationId.Abstractions;

namespace Standard.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly ICorrelationContextAccessor _correlationContext;
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
        ICorrelationContextAccessor correlationContext)
    {
        _logger = logger;
        _next = next;
        _correlationContext = correlationContext;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            var correlationId = _correlationContext.CorrelationContext.CorrelationId;

            _logger.LogError(
                $"Exception Details: {e.Message}, {e.InnerException}, {e.StackTrace}. CorrelationId: {correlationId}");
            throw;
        }
    }
}