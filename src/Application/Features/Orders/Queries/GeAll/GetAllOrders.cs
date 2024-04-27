using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Orders;
using Domain.Shared;
using Serilog;

namespace Application.Features.Orders.Queries.GeAll;

public sealed record GetAllOrdersQuery() : IQuery<List<GetAllOrdersResponse>>;

public sealed class GetAllOrdersQueryHandler : IQueryHandler<GetAllOrdersQuery, List<GetAllOrdersResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetAllOrdersQueryHandler(
        IOrderRepository orderRepository,
        IMapper mapper,
        ILogger logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<List<GetAllOrdersResponse>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);

        var orderResponses = _mapper.Map<List<GetAllOrdersResponse>>(orders);

        _logger.Information($"Returning list of orders in response to GET method" +
                            $" for all orders, status code: 200");

        return orderResponses;
    }
}