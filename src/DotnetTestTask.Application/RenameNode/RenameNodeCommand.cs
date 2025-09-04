using SharedKernel.Application.CQRS;
using SharedKernel.Core.Output;

namespace DotnetTestTask.Application.RenameNode;

public sealed record RenameNodeCommand(string TreeName, long NodeId, string NewNodeName)
    : ICommand;