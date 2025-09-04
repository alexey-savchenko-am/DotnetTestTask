using SharedKernel.Application.CQRS;

namespace DotnetTestTask.Application.DeleteNode;

public sealed record DeleteNodeCommand(string TreeName, long NodeId) : ICommand;