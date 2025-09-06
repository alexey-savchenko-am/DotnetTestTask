using DotnetTestTask.Core.Exceptions;
using DotnetTestTask.Core.TreeAggregate;
using Project.Core.TreeAggregate;
using SharedKernel.Application.CQRS;
using SharedKernel.Core.Output;
using SharedKernel.Core.Persistence;

namespace DotnetTestTask.Application.DeleteNode;

internal sealed class DeleteNodeCommandHandler(
    INodeRepository nodeRepository,
    ISession session)
    : ICommandHandler<DeleteNodeCommand>
{
    public async Task<Result> Handle(DeleteNodeCommand request, CancellationToken cancellationToken)
    {
        // Check if the specified tree exists
        if (!await nodeRepository.RootNodeExistsAsync(request.TreeName))
        {
            throw new SecureException($"Tree with name = '{request.TreeName}' was not found.");
        }

        var node = await nodeRepository.GetByIdAsync(NodeId.Create(request.NodeId));

        if (node is null)
            throw new SecureException($"The requested node with id {request.NodeId} was not found.");

        if (node.Children.Count != 0)
            throw new SecureException($"The requested node with id {request.NodeId} must be a leaf node.");

        nodeRepository.Remove(node);

        await session.StoreAsync(cancellationToken);

        return Result.Success();
    }
}