using MediatR;
using SharedKernel.Application.CQRS;
using SharedKernel.Core.Output;

namespace SharedKernel.Application.CQRS;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{ }

