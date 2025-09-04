namespace DotnetTestTask.Core.JournalAggregate;

public sealed class ExceptionJournal
{
    public JournalId Id { get; private set; } = default!;

    /// <summary>
    /// Уникальный идентификатор события (для ответа клиенту).
    /// </summary>
    public Guid EventId { get; private set; }

    /// <summary>
    /// Время возникновения исключения (UTC).
    /// </summary>
    public DateTime Timestamp { get; private set; }

    /// <summary>
    /// Все query/body параметры запроса (сохраняем как JSON).
    /// </summary>
    public string Parameters { get; private set; } = default!;

    /// <summary>
    /// Полный stack trace исключения.
    /// </summary>
    public string StackTrace { get; private set; } = default!;

    /// <summary>
    /// Тип исключения (Secure или Exception).
    /// </summary>
    public string Type { get; private set; } = default!;

    /// <summary>
    /// Сообщение исключения.
    /// </summary>
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
