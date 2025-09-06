using DotnetTestTask.Application.GetExceptionJournal;
using SharedKernel.Application.CQRS;

namespace DotnetTestTask.Application.GetExceptionJournalRecordById;

public sealed record GetExceptionJournalRecordByIdQuery(long Id)
    : IQuery<JournalRecordDto?>;
