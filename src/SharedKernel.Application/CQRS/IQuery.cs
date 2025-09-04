using MediatR;
using SharedKernel.Core.Output;

namespace SharedKernel.Application.CQRS;

public interface IQuery<TResponse>
    : IRequest<Result<TResponse>>
{ }
