using DotnetTestTask.Application.GetOrCreateTree.Models;
using DotnetTestTask.Core.TreeAggregate;
using SharedKernel.Application.CQRS;

namespace DotnetTestTask.Application.GetOrCreateTree;

public sealed record GetOrCreateTreeCommand(
    string TreeName
) : ICommand<TreeDto?>;
