namespace DotnetTestTask.Application.GetOrCreateTree.Models;

public sealed class NodeDto
{
    public long Id { get; init; }
    public string Name { get; init; } = null!;
    public List<NodeDto> Children { get; init; } = new();
}