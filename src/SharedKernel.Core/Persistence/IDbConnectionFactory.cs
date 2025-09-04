using System.Data;

namespace SharedKernel.Core.Persistence;

public interface IDbConnectionFactory
{
    IDbConnection GetConnection();
    void Return(IDbConnection connection);
}
