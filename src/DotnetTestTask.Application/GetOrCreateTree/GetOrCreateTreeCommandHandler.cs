using Dapper;
using DotnetTestTask.Application.GetOrCreateTree;
using DotnetTestTask.Application.GetOrCreateTree.Models;
using DotnetTestTask.Core.TreeAggregate;
using Project.Core.TreeAggregate;
using SharedKernel.Application.CQRS;
using SharedKernel.Core.Output;
using SharedKernel.Core.Persistence;
using SharedKernel.Persistence;

namespace DotnetTestTask.Application.CreateTree;

internal sealed class GetOrCreateTreeCommandHandler(
    IDbConnectionFactory connectionFactory,
    INodeRepository nodeRepository, 
    ISession session
)
    : ICommandHandler<GetOrCreateTreeCommand, TreeDto?>
{
    public async Task<Result<TreeDto?>> Handle(GetOrCreateTreeCommand request, CancellationToken cancellationToken)
    {
        var existedRootNode = await nodeRepository.GetByNameAsync(request.TreeName, cancellationToken);

        if (existedRootNode is not null)
        {
            return await GetTreeAsync(existedRootNode);
        }

        var rootNode = Node.Create(request.TreeName);

        if (rootNode.IsFailure)
            return rootNode.Error;

        await nodeRepository.AddAsync(rootNode);
        await session.StoreAsync(cancellationToken);

        return new TreeDto
        {
            Id = rootNode.Value.Id.Value,
            Name = rootNode.Value.Name, 
            Children = []
        };
    }


    private async Task<Result<TreeDto?>> GetTreeAsync(Node rootNode)
    {
        using var connection = connectionFactory.GetConnection();

        var sql = """
        WITH RECURSIVE tree_cte AS (
            SELECT n."Id",  n."ParentId", n."Name", 0 AS level
            FROM "nodes" n
            WHERE n."Id" = @rootId AND n."ParentId" IS NULL
            UNION ALL
            SELECT c."Id", c."ParentId", c."Name", p.level + 1
            FROM "nodes" c
            INNER JOIN tree_cte p ON p."Id" = c."ParentId"
        )
        SELECT "Id", "ParentId", "Name"
        FROM tree_cte;
        """
        ;

        var response = await connection.QueryAsync<NodeRecord>(sql, new { rootId = rootNode.Id.Value});

        var result = BuildTree(response);

        return result is null
         ? new Error("Tree.NotFound", "The requested tree was not found")
         : result;
    }

    private static TreeDto? BuildTree(IEnumerable<NodeRecord> flatNodes)
    {
        var nodes = flatNodes
            .ToDictionary(
                r => r.Id,
                r => new NodeDto { Id = r.Id, Name = r.Name });

        foreach (var record in flatNodes)
        {
            if (record.ParentId is not null && nodes.TryGetValue(record.ParentId.Value, out var parent))
            {
                parent.Children.Add(nodes[record.Id]);
            }
        }

        var root = flatNodes.FirstOrDefault(r => r.ParentId is null);
        if (root is null) return null;

        return new TreeDto
        {
            Id = root.Id,
            Name = root.Name,
            Children = nodes[root.Id].Children
        };
    }


    class TreeRecord
    {
        public long Id { get; init; }
        public string Name { get; init; } = null!;
    }

    class NodeRecord
    {
        public long Id { get; init; }
        public long? ParentId { get; init; }
        public string Name { get; init; } = null!;
    }
}
