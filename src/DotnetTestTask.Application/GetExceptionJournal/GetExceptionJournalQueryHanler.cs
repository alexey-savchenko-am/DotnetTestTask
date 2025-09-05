using Dapper;
using DotnetTestTask.Core.JournalAggregate;
using SharedKernel.Application.CQRS;
using SharedKernel.Core.Output;
using SharedKernel.Core.Persistence;
using System.Data;

namespace DotnetTestTask.Application.GetExceptionJournal;

internal sealed class GetExceptionJournalQueryHanler(IDbConnectionFactory connectionFactory)
    : IQueryHandler<GetExceptionJournalQuery, List<JournalRecordDto>>
{
    public async Task<Result<List<JournalRecordDto>>> Handle(GetExceptionJournalQuery request, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.GetConnection();

        var sql = """
        SELECT j."Id", j."EventId", j."Timestamp", j."Parameters", j."StackTrace", j."Type", j."Message"
        FROM exception_journals j
        ORDER BY j."Timestamp" DESC
        OFFSET @Skip LIMIT @Take
        """;

        var response = await connection.QueryAsync<JournalRecordDto>(sql, new
        {
            Skip = (request.Page - 1) * request.PageSize,
            Take = request.PageSize
        });

        return response.ToList();
    }
}
