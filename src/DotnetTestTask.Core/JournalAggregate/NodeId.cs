namespace DotnetTestTask.Core.JournalAggregate;

public sealed record JournalId
{
    public long Value { get; }

    private JournalId(long value)
    {
        Value = value;
    }

    public static JournalId Create(long value) => new(value);

    public override string ToString() => Value.ToString();
}