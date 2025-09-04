using DotnetTestTask.Core.TreeAggregate;

namespace Project.Core.TreeAggregate;

public interface INodeRepository
{
    Task<Node?> GetByNameAsync(string name, CancellationToken ct = default);

    Task<Node?> GetParentNodeAsync(Node node, CancellationToken ct = default);

    Task<Node?> GetByIdAsync(NodeId? id, CancellationToken ct = default);

    Task<IEnumerable<Node>> GetSiblingsAsync(Node node, CancellationToken ct = default);

    Task AddAsync(Node node);

    void Remove(Node node);
}
