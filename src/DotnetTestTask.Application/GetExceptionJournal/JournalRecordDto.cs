
namespace DotnetTestTask.Application.GetExceptionJournal;

public sealed class JournalRecordDto
{
    public long Id { get; set; }

    public Guid EventId { get;  set; }

    public DateTime Timestamp { get; set; }

    public string Parameters { get; set; } = default!;

    public string StackTrace { get; set; } = default!;

    public string Type { get; set; } = default!;

    public string Message { get; set; } = default!;
}