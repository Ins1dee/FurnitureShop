using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Orders;
using Domain.Shared;

namespace Application.Features.Orders.Queries.GetTotalAmountForSelectedYear;

public sealed record GetTotalAmountForYearQuery(int Year) : IQuery<List<GetTotalAmountStatisticResponse>>;

public sealed class GetTotalAmountForYearHandler : IQueryHandler<GetTotalAmountForYearQuery, List<GetTotalAmountStatisticResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public GetTotalAmountForYearHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<List<GetTotalAmountStatisticResponse>>> Handle(GetTotalAmountForYearQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);
        var monthsInYear = Enumerable.Range(1, 12);
        var totalAmountsByYear = monthsInYear
            .GroupJoin(orders.Where(order => order.CreatedAtUtc.Year == request.Year),
                month => month,
                order => order.CreatedAtUtc.Month,
                (month, ordersGroup) => new
                {
                    Month = month,
                    TotalAmount = ordersGroup.Sum(order => order.TotalAmount.Value)
                })
            .Select(result => new GetTotalAmountStatisticResponse(
                result.Month,
                result.TotalAmount))
            .ToList();

        return totalAmountsByYear;
    }
}