using System.Net;
using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Standard.ExampleContext.Application.Dtos;
using Standard.ExampleContext.Application.Facades.Interfaces;
using Standard.ExampleContext.Domain.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace Standard.Api.Controllers;

[Route("api/v1/example")]
[ApiController]
public class ExampleController : ControllerBase
{
    private readonly ICorrelationContextAccessor _correlationContext;
    private readonly IExampleFacade _exampleFacade;
    private readonly ILogger _logger;

    public ExampleController(ICorrelationContextAccessor correlationContext, ILogger<ExampleController> logger,
        IExampleFacade exampleFacade)
    {
        _logger = logger;
        _correlationContext = correlationContext;
        _exampleFacade = exampleFacade;
    }

    [HttpGet]
    [SwaggerResponse((int)HttpStatusCode.OK, "Example successfully returned.",
        typeof(PaginationDto<ExampleResponseDto>))]
    public async Task<ActionResult<PaginationDto<ExampleResponseDto>>> Get(
        [FromQuery] ExampleFilterDto exampleFilterDto)
    {
        try
        {
            var result = await _exampleFacade.GetListByFilterAsync(exampleFilterDto);
            return result;
        }
        catch (ValidationException e)
        {
            _logger.LogError(
                "Exception Details: {message}, {innerException}, {stackTrace}. CorrelationId: {correlationId}",
                e.Message, e.InnerException?.Message, e.StackTrace,
                _correlationContext.CorrelationContext.CorrelationId);

            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid id.")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Example not found.")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Example successfully returned.")]
    public async Task<ActionResult<ExampleResponseDto>> Get(long id)
    {
        try
        {
            if (id <= 0) return BadRequest("Invalid Id.");

            var filter = new ExampleFilterDto { Id = id };
            var result = await _exampleFacade.GetByFilterAsync(filter);

            if (result == null) return NotFound();

            return result;
        }
        catch (ValidationException e)
        {
            _logger.LogError(
                "Exception Details: {message}, {innerException}, {stackTrace}. CorrelationId: {correlationId}",
                e.Message, e.InnerException?.Message, e.StackTrace,
                _correlationContext.CorrelationContext.CorrelationId);

            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid Request.")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Example has been created successfully.")]
    public async Task<IActionResult> Post([FromBody] ExampleRequestDto exampleRequestDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _exampleFacade.CreateAsync(exampleRequestDto);

            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }
        catch (ValidationException e)
        {
            _logger.LogError(
                "Exception Details: {message}, {innerException}, {stackTrace}. CorrelationId: {correlationId}",
                e.Message, e.InnerException?.Message, e.StackTrace,
                _correlationContext.CorrelationContext.CorrelationId);

            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid id.")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Example not found")]
    [SwaggerResponse((int)HttpStatusCode.NoContent, "Example has been updated successfully.")]
    public async Task<IActionResult> Put(long id, [FromBody] ExampleRequestDto exampleRequestDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id <= 0) return BadRequest("Invalid id.");

            await _exampleFacade.UpdateAsync(id, exampleRequestDto);
            return NoContent();
        }
        catch (EntityNotFoundException e)
        {
            _logger.LogError(
                "Exception Details: {message}, {innerException}, {stackTrace}. CorrelationId: {correlationId}",
                e.Message, e.InnerException?.Message, e.StackTrace,
                _correlationContext.CorrelationContext.CorrelationId);

            return NotFound();
        }
        catch (ValidationException e)
        {
            _logger.LogError(
                "Exception Details: {message}, {innerException}, {stackTrace}. CorrelationId: {correlationId}",
                e.Message, e.InnerException?.Message, e.StackTrace,
                _correlationContext.CorrelationContext.CorrelationId);

            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid id.")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Example not found.")]
    [SwaggerResponse((int)HttpStatusCode.NoContent, "Example has been deleted successfully.")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            if (id <= 0) return BadRequest("Invalid id.");

            await _exampleFacade.DeleteAsync(id);

            return NoContent();
        }
        catch (EntityNotFoundException e)
        {
            _logger.LogError(
                "Exception Details: {message}, {innerException}, {stackTrace}. CorrelationId: {correlationId}",
                e.Message, e.InnerException?.Message, e.StackTrace,
                _correlationContext.CorrelationContext.CorrelationId);

            return NotFound();
        }
        catch (ValidationException e)
        {
            _logger.LogError(
                "Exception Details: {message}, {innerException}, {stackTrace}. CorrelationId: {correlationId}",
                e.Message, e.InnerException?.Message, e.StackTrace,
                _correlationContext.CorrelationContext.CorrelationId);

            return BadRequest(e.Message);
        }
    }
}