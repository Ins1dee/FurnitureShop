using Domain.Shared;
using MediatR;

namespace Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponce> : IRequestHandler<TQuery, Result<TResponce>>
    where TQuery : IQuery<TResponce>
{
}