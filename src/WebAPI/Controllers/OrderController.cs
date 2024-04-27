using Application.Features.Orders;
using Application.Features.Orders.Commands.Create;
using Application.Features.Orders.Commands.Delete;
using Application.Features.Orders.GenerateIncoiceInPdf;
using Application.Features.Orders.Queries.GeAll;
using Application.Features.Orders.Queries.Get;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace WebAPI.Controllers;

//[Authorize]
[ApiController]
[Route("api/orders")]
public class OrderController : Controller
{
    private readonly ISender _sender;

    public OrderController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpPost]
    public async Task<IResult> CreateOrder(
        [FromHeader(Name = "X-Idempotency-Key")]
        string requestId,
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(requestId, out Guid parsedRequestId))
        {
            return Results.BadRequest();
        }

        Guid? deliveryId = !string.IsNullOrEmpty(request.DeliveryAddress)
            ? Guid.NewGuid() 
            : null;

        CreateOrderCommand createCommand = new(
            parsedRequestId,
            request.Products,
            request.CustomerName,
            request.CustomerPhone,
            request.PaymentAmount,
            deliveryId,
            request.DeliveryAddress);

        Result result = await _sender.Send(createCommand, cancellationToken);
        
        return result.IsSuccess
            ? Results.Json(null, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }
    
    [HttpDelete]
    public async Task<IResult> DeleteOrder(
        [FromQuery] Guid orderToDeleteId,
        CancellationToken cancellationToken)
    {
        DeleteOrderCommand deleteCommand = new(
            orderToDeleteId);

        Result result = await _sender.Send(deleteCommand, cancellationToken);
        
        return result.IsSuccess
            ? Results.Json(null, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }
    
    [HttpGet]
    [Route("get-user-orders")]
    public async Task<IResult> GetUserOrders(CancellationToken cancellationToken)
    {
        GetOrdersQuery getQuery = new();

        Result<List<OrderResponse>> result = await _sender.Send(getQuery, cancellationToken);
        
        return result.IsSuccess
            ? Results.Json(result.Value, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }

    [HttpGet]
    public async Task<IResult> GetAllOrders(CancellationToken cancellationToken)
    {
        GetAllOrdersQuery getAllQuery = new();

        Result<List<GetAllOrdersResponse>> result = await _sender.Send(getAllQuery, cancellationToken);

        return result.IsSuccess
            ? Results.Json(result.Value, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }

    [HttpGet]
    [Route("invoice-report")]
    public async Task<IResult> GenerateIncoiceReportInPdf(
        [FromHeader(Name = "X-Idempotency-Key")] string requestId, 
        [FromQuery] Guid orderId,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(requestId, out Guid parsedRequestId))
        {
            return Results.BadRequest();
        }

        GenerateIncoiceInPdfCommand generateIncoice = new(parsedRequestId, orderId);

        Result<byte[]> result = await _sender.Send(generateIncoice, cancellationToken);

        return result.IsSuccess
            ? Results.File(result.Value, "application/pdf", $"invoice-{orderId}.pdf")
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }
}