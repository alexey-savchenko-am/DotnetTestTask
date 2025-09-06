using Dapper;
using DotnetTestTask.Application.GetExceptionJournal;
using SharedKernel.Application.CQRS;
using SharedKernel.Core.Output;
using SharedKernel.Core.Persistence;

namespace DotnetTestTask.Application.GetExceptionJournalRecordById;

internal sealed class GetExceptionJournalRecordByIdQueryHandler(IDbConnectionFactory connectionFactory)
    : IQueryHandler<GetExceptionJournalRecordByIdQuery, JournalRecordDto?>
{
    public async Task<Result<JournalRecordDto?>> Handle(GetExceptionJournalRecordByIdQuery request, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.GetConnection();

        var sql = """
            SELECT j."Id", j."EventId", j."Timestamp", j."Parameters", j."StackTrace", j."Type", j."Message"
            FROM exception_journals j
            WHERE j."Id" = @Id
        """;

       var result = await connection.QuerySingleOrDefaultAsync<JournalRecordDto>(sql, 
           new { Id = request.Id });

        return result;
    }
}
