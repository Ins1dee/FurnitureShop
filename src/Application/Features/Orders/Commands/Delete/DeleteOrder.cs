using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Orders;
using Domain.Errors;
using Domain.Shared;

namespace Application.Features.Orders.Commands.Delete;

public sealed record DeleteOrderCommand(
    Guid OrderToDeleteId) : ICommand;

public sealed class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        Order? orderToDelete = await _orderRepository
            .GetByIdAsync(new OrderId(request.OrderToDeleteId), cancellationToken);

        if (orderToDelete is null)
        {
            return Result.NotFound(DomainErrors.Order.NotFound());
        }
        
        _orderRepository.Delete(orderToDelete);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}