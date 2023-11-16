using FurnitureShop.Domain.Shared;
using MediatR;

namespace FurnitureShop.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>, ICommandBase
{
}

public interface ICommand<TResponce> : IRequest<Result<TResponce>>, ICommandBase
{
}

public interface ICommandBase
{
}