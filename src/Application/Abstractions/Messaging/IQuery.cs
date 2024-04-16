using Domain.Shared;
using MediatR;

namespace Application.Abstractions.Messaging;

public interface IQuery<TResponce> : IRequest<Result<TResponce>>
{
}