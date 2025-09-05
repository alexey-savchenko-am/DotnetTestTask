using DotnetTestTask.Core.TreeAggregate;
using Project.Core.TreeAggregate;
using SharedKernel.Application.CQRS;
using SharedKernel.Core.Output;
using SharedKernel.Core.Persistence;

namespace DotnetTestTask.Application.RenameNode;

internal sealed class RenameNodeCommandHandler(INodeRepository nodeRepository, ISession session)
    : ICommandHandler<RenameNodeCommand>
{
    public async Task<Result> Handle(RenameNodeCommand request, CancellationToken cancellationToken)
    {
        var node = await nodeRepository.GetByIdAsync(NodeId.Create(request.NodeId));

        if (node is null)
            return new Error("Tree.NotFound", $"Node '{request.NodeId}' was not found.");

        var currentNodeSiblings = await nodeRepository.GetSiblingsAsync(node, cancellationToken);

        var hasDuplicate = currentNodeSiblings.Any(n => node.Name == request.NewNodeName);

        if (hasDuplicate)
        {
            return new Error("Node.DuplicateName",
                $"Node with name '{request.NewNodeName}' already exists among siblings.");
        }

        var renameNodeResult = node.Rename(request.NewNodeName);

        if (renameNodeResult.IsFailure)
            return renameNodeResult.Error;

        await session.StoreAsync(cancellationToken);

        return Result.Success();
    }
}