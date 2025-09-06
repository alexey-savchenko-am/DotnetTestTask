using Dapper;
using DotnetTestTask.Core.JournalAggregate;
using SharedKernel.Application.CQRS;
using SharedKernel.Core.Output;
using SharedKernel.Core.Persistence;
using System.Data;
using System.Linq;
using System.Text;

namespace DotnetTestTask.Application.GetExceptionJournal;

internal sealed class GetExceptionJournalQueryHandler(IDbConnectionFactory connectionFactory)
    : IQueryHandler<GetExceptionJournalQuery, List<JournalRecordDto>>
{
    public async Task<Result<List<JournalRecordDto>>> Handle(GetExceptionJournalQuery request, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.GetConnection();

        var sql = new StringBuilder("""
            SELECT j."Id", j."EventId", j."Timestamp", j."Parameters", j."StackTrace", j."Type", j."Message"
            FROM exception_journals j
            WHERE 1=1
        """);

        var parameters = new DynamicParameters();
        parameters.Add("Skip", request.Skip, DbType.Int32);
        parameters.Add("Take", request.Take, DbType.Int32);

        if (request.FromDate.HasValue)
        {
            sql.Append(" AND j.\"Timestamp\" >= @FromDate");
            parameters.Add("FromDate", request.FromDate.Value, DbType.DateTime);
        }

        if (request.ToDate.HasValue)
        {
            sql.Append(" AND j.\"Timestamp\" <= @ToDate");
            parameters.Add("ToDate", request.ToDate.Value, DbType.DateTime);
        }

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            sql.Append(" AND (j.\"Message\" ILIKE @Keyword OR j.\"StackTrace\" ILIKE @Keyword OR j.\"Parameters\" ILIKE @Keyword)");
            parameters.Add("Keyword", $"%{request.Keyword}%", DbType.String);
        }

        sql.Append(" ORDER BY j.\"Timestamp\" DESC OFFSET @Skip LIMIT @Take");

        var response = await connection.QueryAsync<JournalRecordDto>(sql.ToString(), parameters);

        return response.ToList();
    }
}
