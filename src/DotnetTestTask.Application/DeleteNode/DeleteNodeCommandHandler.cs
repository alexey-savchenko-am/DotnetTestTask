using DotnetTestTask.Core.TreeAggregate;
using Project.Core.TreeAggregate;
using SharedKernel.Application.CQRS;
using SharedKernel.Core.Output;
using SharedKernel.Persistence;

namespace DotnetTestTask.Application.DeleteNode;

internal sealed class DeleteNodeCommandHandler(
    INodeRepository nodeRepository,
    ISession session)
    : ICommandHandler<DeleteNodeCommand>
{
    public async Task<Result> Handle(DeleteNodeCommand request, CancellationToken cancellationToken)
    {
        var node = await nodeRepository.GetByIdAsync(NodeId.Create(request.NodeId));

        if (node is null)
            return new Error("Node.NotFound", $"The requested node with id {request.NodeId} was not found.");

        if (node.Children.Count != 0)
          return new Error("Node.NotLeaf", $"The requested node with id {request.NodeId} must be a leaf node.");

        try
        {
            nodeRepository.Remove(node);

            await session.StoreAsync(cancellationToken);
        }
        catch (Exception ex)
        {

        }

        return Result.Success();
    }
}