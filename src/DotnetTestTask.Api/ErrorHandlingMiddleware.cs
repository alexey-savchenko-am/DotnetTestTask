using DotnetTestTask.Core.JournalAggregate;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Core.Persistence;

namespace DotnetTestTask.Api;

public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly SharedKernel.Core.Persistence.ISession _session;

    public ErrorHandlingMiddleware(RequestDelegate next, SharedKernel.Core.Persistence.ISession session)
    {
        _next = next;
        _session = session;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var eventId = Guid.NewGuid();
            var allParamsJson = await context.GetRequestParametersAsJsonAsync();

            var journal = ExceptionJournal.CreateGeneral(
                eventId,
                ex.Message,
                allParamsJson,
                ex.ToString()
            );

            await _session.AddAsync(journal);
            await _session.StoreAsync();

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new
            {
                type = "Exception",
                id = eventId,
                data = new { message = $"Internal server error ID = {eventId}" }
            });
        }
    }
}
