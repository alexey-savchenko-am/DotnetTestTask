using SharedKernel.Application.CQRS;

namespace DotnetTestTask.Application.GetExceptionJournal;

public sealed record GetExceptionJournalQuery(
    int Skip, 
    int Take, 
    DateTime? FromDate, 
    DateTime? ToDate,
    string? Keyword) 
    : IQuery<List<JournalRecordDto>>;