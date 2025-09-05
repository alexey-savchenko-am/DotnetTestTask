using DotnetTestTask.Application.TreeNodes.CreateNode;
using DotnetTestTask.Core.TreeAggregate;
using Project.Core.TreeAggregate;
using SharedKernel.Application.CQRS;
using SharedKernel.Core.Output;
using SharedKernel.Core.Persistence;

namespace DotnetTestTask.Application.CreateNode;

internal sealed class CreateNodeCommandHandler(INodeRepository nodeRepository, ISession session)
    : ICommandHandler<CreateNodeCommand, NodeId>
{
    public async Task<Result<NodeId>> Handle(CreateNodeCommand request, CancellationToken cancellationToken)
    {
        var parentNodeId = NodeId.Create(request.ParentNodeId);

        var parentNode = await nodeRepository.GetByIdAsync(parentNodeId, cancellationToken);

        if (parentNode is null)
            return new Error("Node.NotFound", $"Parent node with Id {request.ParentNodeId} was not found.");

        var newNodeResult = Node.Create(request.NodeName, parentNode);

        if (newNodeResult.IsFailure)
            return newNodeResult.Error;

        await session.StoreAsync(cancellationToken);

        return newNodeResult.Value.Id;
    }
}
