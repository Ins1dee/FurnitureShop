using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Deliveries;
using Domain.Errors;
using Domain.Shared;
using Serilog;

namespace Application.Features.Deliveries.Commands.AttachToLoggedUser;

public sealed record AttachDeliveryToLoggedUserCommand(Guid DeliveryId) : ICommand;

public sealed class AttachDeliveryToLoggedUserCommandHandler : ICommandHandler<AttachDeliveryToLoggedUserCommand>
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly ISessionService _sessionService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public AttachDeliveryToLoggedUserCommandHandler(
        IDeliveryRepository deliveryRepository,
        ISessionService sessionService,
        IUnitOfWork unitOfWork, 
        ILogger logger)
    {
        _deliveryRepository = deliveryRepository;
        _sessionService = sessionService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(AttachDeliveryToLoggedUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _sessionService.GetLoggedInUserAsync(cancellationToken);

        if (user is null)
        {
            _logger.Error($"Error occured trying to attach delivery to user. " +
                          $"Error message: {DomainErrors.User.Unauthorized()}. " +
                          $"Status code: 401");

            return Result.Unauthorized(DomainErrors.User.Unauthorized());
        }

        var delivery = await _deliveryRepository.GetByIdAsync(new DeliveryId(request.DeliveryId), cancellationToken);

        if (delivery is null)
        {
            _logger.Error($"Error occured trying to attach delivery with id {request.DeliveryId} to logged user. " +
                          $"Error message: {DomainErrors.Delivery.NotFound()}. " +
                          $"Status code: 404");

            return Result.NotFound(DomainErrors.Delivery.NotFound());
        }

        Result attachResult = delivery.AttachToUser(user.Id);

        if (attachResult.IsFailure)
        {
            _logger.Error($"Error occured trying to attach delivery with id {request.DeliveryId} to logged user. " +
                          $"Error message: {attachResult.Error}. " +
                          $"Status code: {(int)attachResult.Status}");

            return attachResult;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information($"Delivery with id {request.DeliveryId} was successfully attached" +
                            $" to logged user, status code: 200");
        
        return Result.Success();
    }
}