namespace SharedKernel.Core.Persistence;

public interface IDatabaseInitializer
{
    Task<bool> InitializeWithTestDataAsync(bool recreateDatabase);
}
