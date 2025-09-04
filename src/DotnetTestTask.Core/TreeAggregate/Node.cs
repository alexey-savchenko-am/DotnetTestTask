using SharedKernel.Core.Output;

namespace DotnetTestTask.Core.TreeAggregate;

public sealed class Node
{
    public NodeId Id { get; private set; } = default!;

    public Guid? TreeId { get; private set; }

    public NodeId? ParentId { get; private set; }

    public Node? Parent { get; private set; }

    private readonly List<Node> _children = new();
    public IReadOnlyCollection<Node> Children => _children.AsReadOnly();

    public string Name { get; private set; } = default!;

    #pragma warning disable CS8618
    private Node() { }
    #pragma warning restore

    private Node(string name, Node? parent = null)
    {
        Rename(name);

        if (parent != null)
        {
            Parent = parent;
            ParentId = parent.Id;
            parent._children.Add(this);
        } 
        else
        {
            // root node for a tree
            TreeId = Guid.NewGuid();    
        }
    }

    public static Result<Node> Create(string name, Node? parent = null)
    {
        if(string.IsNullOrWhiteSpace(name))
            return new Error("Node.NameRequired", "Node name required.");
        var node = new Node(name, parent);
        return node;
    }

    public void AddChild(Node childNode)
    {
        if(childNode is null) 
            throw new ArgumentNullException(nameof(childNode), "Node cannot be null");

        _children.Add(childNode);
    }

    public Result Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            return new Error("NodeName.Empty", "Node must have a valid name.");

        Name = newName;

        return Result.Success();
    }
}