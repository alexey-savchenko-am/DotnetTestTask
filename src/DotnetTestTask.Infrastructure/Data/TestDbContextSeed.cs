using DotnetTestTask.Core.TreeAggregate;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Core.Persistence;

namespace DotnetTestTask.Infrastructure.Data;

internal sealed class TestDbContextSeed(AppDbContext dbContext) : IDatabaseInitializer
{
    public async Task<bool> InitializeWithTestDataAsync(bool recreateDatabase)
    {
        if (recreateDatabase)
        {
            dbContext.Database.EnsureDeleted();
            await dbContext.Database.MigrateAsync();
        }

        var tree = BuildTestTree();
        dbContext.Set<Node>().Add(tree);
        await dbContext.SaveChangesAsync();

        return true;
    }

    private static Node BuildTestTree()
    {
        var treeRoot = Node.Create("CompanyTree");

        // Основные департаменты
        var itNode = Node.Create("IT Department", treeRoot);
        var salesNode = Node.Create("Sales Department", treeRoot);
        var hrNode = Node.Create("HR Department", treeRoot);

        // IT 
        var backendNode = Node.Create("Backend Team", itNode);
        var frontEndNode = Node.Create("Frontend Team", itNode);
        var devopsNode = Node.Create("DevOps Team", itNode);

        // Backend 
        Node.Create("Ann Doe", backendNode);
        Node.Create("John Smith", backendNode);

        // Frontend 
        Node.Create("Michael Brown", frontEndNode);
        Node.Create("Emily Davis", frontEndNode);

        // DevOps 
        Node.Create("Robert Wilson", devopsNode);
        Node.Create("Laura Johnson", devopsNode);

        Node.Create("Kevin White", salesNode);
        Node.Create("Sophia Green", salesNode);

        Node.Create("Olivia Taylor", hrNode);
        Node.Create("James Miller", hrNode);

        return treeRoot;
    }
}

