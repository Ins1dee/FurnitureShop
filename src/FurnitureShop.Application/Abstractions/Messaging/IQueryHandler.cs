using FurnitureShop.Domain.Shared;
using MediatR;

namespace FurnitureShop.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponce> : IRequestHandler<TQuery, Result<TResponce>>
    where TQuery : IQuery<TResponce>
{
}