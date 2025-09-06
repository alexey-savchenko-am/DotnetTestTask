using DotnetTestTask.Core.TreeAggregate;
using DotnetTestTask.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Project.Core.TreeAggregate;

namespace DotnetTestTask.Infrastructure.Repositories;

internal sealed class NodeRepository(AppDbContext dbContext) : INodeRepository
{
    public async Task<bool> RootNodeExistsAsync(string name, CancellationToken ct = default)
    {
        return await dbContext.Nodes
          .AnyAsync(n => n.Name == name && n.Parent == null, ct);
    }

    public async Task<Node?> GetByNameAsync(string name, CancellationToken ct = default)
    {
        return await dbContext.Nodes
            .Include(n => n.Children) 
            .FirstOrDefaultAsync(n => n.Name == name, ct);
    }

    public async Task<Node?> GetParentNodeAsync(Node node, CancellationToken ct = default)
    {
        if (node.ParentId is null)
            return null;

        return await dbContext.Nodes
            .Include(n => n.Children) 
            .FirstOrDefaultAsync(n => n.Id == node.ParentId, ct);
    }

    public async Task<Node?> GetByIdAsync(NodeId? id, CancellationToken ct = default)
    {
        if (id is null)
            return null;

        return await dbContext.Nodes
            .Include(n => n.Children)
            .FirstOrDefaultAsync(n => n.Id == id, ct);
    }

    public async Task<IEnumerable<Node>> GetSiblingsAsync(Node node, CancellationToken ct = default)
    {
        if (node.ParentId is null)
            return [];

        return await dbContext.Nodes
            .Include(n => n.Children) 
            .Where(n => n.ParentId == node.ParentId && n.Id != node.Id)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Node node)
    {
       await dbContext.Nodes.AddAsync(node);   
    }

    public void Remove(Node node)
    {
        dbContext.Nodes.Remove(node);
    }
}
