using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Orders;
using Domain.Entities.Users;
using Domain.Errors;
using Domain.Shared;

namespace Application.Features.Orders.Queries.Get;

public sealed record GetOrdersQuery : IQuery<List<OrderResponse>>;

public sealed class GetOrderQueryHandler : IQueryHandler<GetOrdersQuery, List<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISessionService _sessionService;
    private readonly IMapper _mapper;

    public GetOrderQueryHandler(IOrderRepository orderRepository, ISessionService sessionService, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _sessionService = sessionService;
        _mapper = mapper;
    }

    public async Task<Result<List<OrderResponse>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        User? user = await _sessionService.GetLoggedInUserAsync(cancellationToken);

        if (user is null)
        {
            return Result.Unauthorized<List<OrderResponse>>(DomainErrors.User.Unauthorized());
        }
        
        List<Order> orders = await _orderRepository.GetAsync(user.Id, cancellationToken);

        var orderResponses = _mapper.Map<List<OrderResponse>>(orders);

        return orderResponses;
    }
}