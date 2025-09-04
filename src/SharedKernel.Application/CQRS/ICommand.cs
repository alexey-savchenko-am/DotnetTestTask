using MediatR;
using SharedKernel.Core.Output;

namespace SharedKernel.Application.CQRS;

public interface ICommand
    : IRequest<Result>
{ }

public interface ICommand<TResponse>
    : IRequest<Result<TResponse>>
{ }
