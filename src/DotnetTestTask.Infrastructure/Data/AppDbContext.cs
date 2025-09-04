using DotnetTestTask.Core.JournalAggregate;
using DotnetTestTask.Core.TreeAggregate;
using Microsoft.EntityFrameworkCore;

namespace DotnetTestTask.Infrastructure.Data;

internal sealed class AppDbContext : DbContext
{
    public DbSet<Node> Nodes { get; set; }
    public DbSet<ExceptionJournal> ExceptionJournals { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
