using DotnetTestTask.Core.TreeAggregate;
using SharedKernel.Application.CQRS;
using SharedKernel.Core.Output;

namespace DotnetTestTask.Application.TreeNodes.CreateNode;

public sealed record CreateNodeCommand(
    string TreeName,
    long ParentNodeId,
    string NodeName
) : ICommand<NodeId>; 
