using SharedKernel.Application.CQRS;

namespace DotnetTestTask.Application.GetExceptionJournal;

public sealed record GetExceptionJournalQuery(int Page, int PageSize) 
    : IQuery<List<JournalRecordDto>>;