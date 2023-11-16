using FurnitureShop.Application.Features.Products.Commands.Create;
using FurnitureShop.Domain.Shared;
using FurnitureShop.WebAPI.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.WebAPI.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : Controller
{
    private readonly ISender _sender;

    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        [FromHeader(Name = "X-Idempotency-Key")] string requestId,
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(requestId, out Guid parsedRequestId))
        {
            return BadRequest();
        }

        CreateProductCommand createCommand = new(
            parsedRequestId,
            request.Name,
            request.Description,
            request.BrandCompany,
            request.BrandCountry,
            request.Price,
            request.Width,
            request.Height,
            request.Length,
            request.Categories);

        Result result = await _sender.Send(createCommand, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}