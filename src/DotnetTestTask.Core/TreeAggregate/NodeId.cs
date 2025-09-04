namespace DotnetTestTask.Core.TreeAggregate;

public sealed record NodeId
{
    public long Value { get; }

    private NodeId(long value)
    {
        Value = value;
    }

    public static NodeId Create(long value) => new(value);

    public override string ToString() => Value.ToString();
}