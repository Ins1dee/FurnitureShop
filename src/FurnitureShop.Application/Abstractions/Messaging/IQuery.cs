using FurnitureShop.Domain.Shared;
using MediatR;

namespace FurnitureShop.Application.Abstractions.Messaging;

public interface IQuery<TResponce> : IRequest<Result<TResponce>>
{
}