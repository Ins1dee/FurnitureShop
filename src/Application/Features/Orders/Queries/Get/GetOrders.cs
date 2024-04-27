using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Orders;
using Domain.Entities.Users;
using Domain.Errors;
using Domain.Shared;
using Serilog;

namespace Application.Features.Orders.Queries.Get;

public sealed record GetOrdersQuery : IQuery<List<OrderResponse>>;

public sealed class GetOrderQueryHandler : IQueryHandler<GetOrdersQuery, List<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISessionService _sessionService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetOrderQueryHandler(
        IOrderRepository orderRepository,
        ISessionService sessionService,
        IMapper mapper,
        ILogger logger)
    {
        _orderRepository = orderRepository;
        _sessionService = sessionService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<List<OrderResponse>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        User? user = await _sessionService.GetLoggedInUserAsync(cancellationToken);

        if (user is null)
        {
            _logger.Error($"Error occured trying to get orders of logged user. " +
                          $"Error message: {DomainErrors.User.Unauthorized()}. " +
                          $"Status code: 401");

            return Result.Unauthorized<List<OrderResponse>>(DomainErrors.User.Unauthorized());
        }
        
        List<Order> orders = await _orderRepository.GetAsync(user.Id, cancellationToken);

        var orderResponses = _mapper.Map<List<OrderResponse>>(orders);

        _logger.Information($"Returning list of orders for logged user in response to GET method" +
                            $" , status code: 200");

        return orderResponses;
    }
}