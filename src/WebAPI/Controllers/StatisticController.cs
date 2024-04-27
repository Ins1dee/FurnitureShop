using Application.Features.Orders;
using Application.Features.Orders.Queries.GetTotalAmountForSelectedYear;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace WebAPI.Controllers;


[ApiController]
[Route("api/statistic")]
public class StatisticController : Controller
{
    private readonly ISender _sender;

    public StatisticController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IResult> GetOrderStatistic([FromQuery] int year, CancellationToken cancellationToken)
    {
        GetTotalAmountForYearQuery query = new(year);

        Result<List<GetTotalAmountStatisticResponse>> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Json(result.Value, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }
}
