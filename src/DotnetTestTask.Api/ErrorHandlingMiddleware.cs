using DotnetTestTask.Core.Exceptions;
using DotnetTestTask.Core.JournalAggregate;

namespace DotnetTestTask.Api;

public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ExceptionJournal? journalRecord = null;

        try
        {
            await _next(context);
        }
        catch (SecureException ex)
        {
            var requestParams = await context.GetRequestParametersAsJsonAsync();

            journalRecord = ExceptionJournal.CreateSecure(
                Guid.NewGuid(),
                ex.Message,
                requestParams,
                ex.ToString()
            );

            await CreateResponse(context, journalRecord);
        }
        catch (Exception ex)
        {
            var requestParams = await context.GetRequestParametersAsJsonAsync();

            journalRecord = ExceptionJournal.CreateGeneral(
                Guid.NewGuid(),
                ex.Message,
                requestParams,
                ex.ToString()
            );

            await CreateResponse(context, journalRecord);
        }
        finally
        {
            if (journalRecord is not null)
            {
                var session = context.RequestServices.GetRequiredService<SharedKernel.Core.Persistence.ISession>();
                await session.AddAsync(journalRecord);
                await session.StoreAsync();
            }
        }
    }

    private static async Task CreateResponse(HttpContext context, ExceptionJournal journalRecord)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            type = journalRecord.Type,
            id = journalRecord.EventId,
            data = new
            {
                message = journalRecord.Type == "Exception"
                        ? $"Internal server error ID = {journalRecord.EventId}"
                        : journalRecord.Message
            }
        });
    }
}
