using Application.Features.Users.Commands.Login;
using Application.Features.Users.Commands.RefreshCurrentSession;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : Controller
{
    private readonly ISender _sender;

    public AuthenticationController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IResult> LoginUser(
        [FromHeader(Name = "X-Idempotency-Key")] string requestId,
        [FromBody] LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(requestId, out Guid parsedRequestId))
        {
            return Results.BadRequest();
        }
        
        LoginUserCommand loginCommand = new(
            parsedRequestId,
            request.Email,
            request.Password);

        Result<LoginResponse> result = await _sender.Send(loginCommand, cancellationToken);

        return result.IsSuccess
            ? Results.Json(result.Value, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }

    [Authorize]
    [HttpPost]
    [Route("refresh-token")]
    public async Task<IResult> RefreshToken(CancellationToken cancellationToken)
    {
        RefreshSessionCommand refreshCommand = new();

        Result<RefreshSessionResponse> result = await _sender.Send(refreshCommand, cancellationToken);
        
        return result.IsSuccess 
            ? Results.Json(result.Value, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }
}