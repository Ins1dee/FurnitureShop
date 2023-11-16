using FurnitureShop.Domain.Shared;
using MediatR;

namespace FurnitureShop.Application.Abstractions.Messaging;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResponce> : IRequestHandler<TCommand, Result<TResponce>>
    where TCommand : ICommand<TResponce>
{
}