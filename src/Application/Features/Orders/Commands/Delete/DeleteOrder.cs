using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Orders;
using Domain.Errors;
using Domain.Shared;
using Serilog;

namespace Application.Features.Orders.Commands.Delete;

public sealed record DeleteOrderCommand(
    Guid OrderToDeleteId) : ICommand;

public sealed class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public DeleteOrderCommandHandler(
        IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork, 
        ILogger logger)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        Order? orderToDelete = await _orderRepository
            .GetByIdAsync(new OrderId(request.OrderToDeleteId), cancellationToken);

        if (orderToDelete is null)
        {
            _logger.Error($"Error occured trying to delete order with id {request.OrderToDeleteId}. " +
                          $"Error message: {DomainErrors.Order.NotFound()}. " +
                          $"Status code: 404");

            return Result.NotFound(DomainErrors.Order.NotFound());
        }
        
        _orderRepository.Delete(orderToDelete);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information($"Order with id {request.OrderToDeleteId} was successfully deleted, status code: 200");

        return Result.Success();
    }
}