using Application.Features.Deliveries;
using Application.Features.Deliveries.Commands.AttachToLoggedUser;
using Application.Features.Deliveries.Queries.Get;
using Application.Features.Deliveries.Queries.GetAll;
using Domain.Entities.Roles;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/deliveries")]
public class DeliveryController
{
    private readonly ISender _sender;

    public DeliveryController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Roles = nameof(Role.Delivery))]
    [HttpPatch]
    [Route("attach")]
    public async Task<IResult> AttachToUser([FromQuery] Guid deliveryId, CancellationToken cancellationToken)
    {
        AttachDeliveryToLoggedUserCommand attachCommand = new(deliveryId);

        Result result = await _sender.Send(attachCommand, cancellationToken);

        return result.IsSuccess
            ? Results.Json(null, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }

    [Authorize(Roles = nameof(Role.Delivery))]
    [HttpGet]
    [Route("get-user-deliveries")]
    public async Task<IResult> GetDeliveries(CancellationToken cancellationToken)
    {
        GetDeliveriesQuery getDeliveriesQuery = new();

        Result<List<DeliveryResponse>> result = await _sender.Send(getDeliveriesQuery, cancellationToken);

        return result.IsSuccess
            ? Results.Json(result.Value, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }

    [HttpGet]
    public async Task<IResult> GetAllDeliveries(CancellationToken cancellationToken)
    {
        GetAllDeliveriesQuery getAllDeliveriesQuery = new();

        Result<List<DeliveryResponse>> result = await _sender.Send(getAllDeliveriesQuery, cancellationToken);

        return result.IsSuccess
            ? Results.Json(result.Value, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }
}
