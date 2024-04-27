using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Deliveries;
using Domain.Shared;
using Serilog;

namespace Application.Features.Deliveries.Queries.GetAll;

public sealed record GetAllDeliveriesQuery : IQuery<List<DeliveryResponse>>;

public sealed class GetAllDeliveriesQueryHandler : IQueryHandler<GetAllDeliveriesQuery, List<DeliveryResponse>>
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetAllDeliveriesQueryHandler(
        IDeliveryRepository deliveryRepository,
        IMapper mapper,
        ILogger logger)
    {
        _deliveryRepository = deliveryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<List<DeliveryResponse>>> Handle(GetAllDeliveriesQuery request, CancellationToken cancellationToken)
    {
        var deliveries = await _deliveryRepository.GetAllAsync(cancellationToken);

        var deliveryResponses = _mapper.Map<List<DeliveryResponse>>(deliveries);

        _logger.Information($"Returning list of deliveries in response to GET method" +
                            $" for all deliveries, status code: 200");

        return deliveryResponses;
    }
}