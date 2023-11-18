using FurnitureShop.Application.Features.Products;
using FurnitureShop.Application.Features.Products.Commands.Create;
using FurnitureShop.Application.Features.Products.Commands.Delete;
using FurnitureShop.Application.Features.Products.Commands.Update;
using FurnitureShop.Application.Features.Products.Queries.Get;
using FurnitureShop.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

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
    public async Task<IResult> CreateProduct(
        [FromHeader(Name = "X-Idempotency-Key")] string requestId,
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(requestId, out Guid parsedRequestId))
        {
            return Results.BadRequest();
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

        return result.IsSuccess
            ? Results.Json(null, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }

    [HttpGet]
    public async Task<IResult> GetProducts(CancellationToken cancellationToken)
    {
        GetProductsQuery getQuery = new();

        Result<List<ProductResponse>> result = await _sender.Send(getQuery, cancellationToken);
        
        return result.IsSuccess 
            ? Results.Json(result.Value, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }

    [HttpPut]
    public async Task<IResult> UpdateProduct(
        [FromQuery] Guid productToDeleteId,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        UpdateProductCommand updateCommand = new(
            productToDeleteId,
            request.Name,
            request.Description,
            request.BrandCompany,
            request.BrandCountry,
            request.Price,
            request.Width,
            request.Height,
            request.Length,
            request.Categories);

        Result result = await _sender.Send(updateCommand, cancellationToken);
        
        return result.IsSuccess
            ? Results.Json(null, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }
    
    [HttpDelete]
    public async Task<IResult> DeleteProduct(
        [FromQuery] Guid productToDeleteId,
        CancellationToken cancellationToken)
    {
        DeleteProductCommand deleteCommand = new(
            productToDeleteId);

        Result result = await _sender.Send(deleteCommand, cancellationToken);
        
        return result.IsSuccess
            ? Results.Json(null, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }
}