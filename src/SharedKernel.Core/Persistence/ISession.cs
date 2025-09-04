using SharedKernel.Core.Output;
using System.Data;

namespace SharedKernel.Persistence;

public interface ISession
{
    IDbTransaction StartTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

    Task<Result<TResult>> ExecuteWithinTransaction<TResult>(
        Func<Task<Result<TResult>>> action,
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken ct = default);

    Task<Result<TResult>> ExecuteWithinReadCommitedTransaction<TResult>(Func<Task<Result<TResult>>> action, CancellationToken ct = default);
    Task<Result<TResult>> ExecuteWithinRepeatableReadTransaction<TResult>(Func<Task<Result<TResult>>> action, CancellationToken ct = default);

    Task StoreAsync(CancellationToken ct = default);
}
