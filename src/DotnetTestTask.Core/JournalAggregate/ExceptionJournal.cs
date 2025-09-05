namespace DotnetTestTask.Core.JournalAggregate;

public sealed class ExceptionJournal
{
    public JournalId Id { get; private set; } = default!;

    public Guid EventId { get; private set; }

    public DateTime Timestamp { get; private set; }

    public string Parameters { get; private set; } = default!;

    public string StackTrace { get; private set; } = default!;

    public string Type { get; private set; } = default!;

    public string Message { get; private set; } = default!;

#pragma warning disable CS8618
    private ExceptionJournal() { }
#pragma warning restore

    private ExceptionJournal(Guid eventId, string type, string message, string parameters, string stackTrace)
    {
        EventId = eventId;
        Timestamp = DateTime.UtcNow;
        Type = type;
        Message = message;
        Parameters = parameters;
        StackTrace = stackTrace;
    }

    public static ExceptionJournal CreateSecure(Guid eventId, string message, string parameters, string stackTrace)
        => new(eventId, "Secure", message, parameters, stackTrace);

    public static ExceptionJournal CreateGeneral(Guid eventId, string message, string parameters, string stackTrace)
        => new(eventId, "Exception", message, parameters, stackTrace);
}
