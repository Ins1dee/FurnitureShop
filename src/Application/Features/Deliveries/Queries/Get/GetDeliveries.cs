using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Deliveries;
using Domain.Errors;
using Domain.Shared;
using Serilog;

namespace Application.Features.Deliveries.Queries.Get;

public sealed record GetDeliveriesQuery : IQuery<List<DeliveryResponse>>;

public sealed class GetDeliveriesQueryHandler : IQueryHandler<GetDeliveriesQuery, List<DeliveryResponse>>
{

    private readonly IDeliveryRepository _deliveryRepository;
    private readonly ISessionService _sessionService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetDeliveriesQueryHandler(
        IDeliveryRepository deliveryRepository,
        ISessionService sessionService,
        IMapper mapper,
        ILogger logger)
    {
        _deliveryRepository = deliveryRepository;
        _sessionService = sessionService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<List<DeliveryResponse>>> Handle(GetDeliveriesQuery request, CancellationToken cancellationToken)
    {
        var user = await _sessionService.GetLoggedInUserAsync(cancellationToken);

        if (user is null)
        {
            _logger.Error($"Error occured trying to get deliveries of logged user. " +
                          $"Error message: {DomainErrors.User.Unauthorized()}. " +
                          $"Status code: 401");

            return Result.Unauthorized<List<DeliveryResponse>>(DomainErrors.User.Unauthorized());
        }

        var deliveries = await _deliveryRepository.GetAsync(user.Id, cancellationToken);

        var deliveryResponses = _mapper.Map<List<DeliveryResponse>>(deliveries);

        _logger.Information($"Returning list of deliveries for logged user in response to GET method" +
                            $" , status code: 200");

        return deliveryResponses;
    }
}
