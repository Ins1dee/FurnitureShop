using Application.Features.Categories;
using Application.Features.Categories.Commands.Create;
using Application.Features.Categories.Commands.Delete;
using Application.Features.Categories.Commands.Update;
using Application.Features.Categories.Queries.Get;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace WebAPI.Controllers;

[Authorize(Roles = "Administrator")]
[ApiController]
[Route("api/categories")]
public class CategoryController : Controller
{
    private readonly ISender _sender;

    public CategoryController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpPost]
    public async Task<IResult> CreateCategory(
        [FromBody] CategoryRequest request,
        CancellationToken cancellationToken)
    {
        CreateCategoryCommand createCommand = new(request.Name);

        Result result = await _sender.Send(createCommand, cancellationToken);
        
        return result.IsSuccess
            ? Results.Json(null, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IResult> GetCategories(CancellationToken cancellationToken)
    {
        GetCategoriesQuery getQuery = new();

        var result = await _sender.Send(getQuery, cancellationToken);
        
        return result.IsSuccess
            ? Results.Json(result.Value, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }

    [HttpPut]
    public async Task<IResult> UpdateCategory(
        [FromQuery] Guid categoryToUpdateId,
        [FromBody] CategoryRequest request,
        CancellationToken cancellationToken)
    {
        UpdateCategoryCommand updateCommand = new(categoryToUpdateId, request.Name);

        Result result = await _sender.Send(updateCommand, cancellationToken);
        
        return result.IsSuccess
            ? Results.Json(null, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }
    
    [HttpDelete]
    public async Task<IResult> DeleteCategory(
        [FromQuery] Guid categoryToDeleteId,
        CancellationToken cancellationToken)
    {
        DeleteCategoryCommand deleteCommand = new(categoryToDeleteId);

        Result result = await _sender.Send(deleteCommand, cancellationToken);
        
        return result.IsSuccess
            ? Results.Json(null, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }
}