using DotnetTestTask.Application.TreeNodes.CreateNode;
using DotnetTestTask.Core.Exceptions;
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
        // Check if the specified tree exists
        if(!await nodeRepository.RootNodeExistsAsync(request.TreeName))
        {
           throw new SecureException($"Tree with name = '{request.TreeName}' was not found.");
        }

        var parentNode = await nodeRepository.GetByIdAsync(
            NodeId.Create(request.ParentNodeId), cancellationToken);

        if (parentNode is null)
            throw new SecureException($"Parent node with id = {request.ParentNodeId} was not found.");

        var newNodeResult = Node.Create(request.NodeName, parentNode);

        if (newNodeResult.IsFailure)
            throw new SecureException(newNodeResult.Error!);

        await session.StoreAsync(cancellationToken);

        return newNodeResult.Value.Id;
    }
}
