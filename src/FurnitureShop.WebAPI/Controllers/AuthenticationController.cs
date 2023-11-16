using FurnitureShop.Application.Features.Users.Commands.Login;
using FurnitureShop.Application.Features.Users.Commands.RefreshSession;
using FurnitureShop.Domain.Shared;
using FurnitureShop.WebAPI.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.WebAPI.Controllers;

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
    public async Task<IActionResult> LoginUser(
        [FromHeader(Name = "X-Idempotency-Key")] string requestId,
        [FromBody] LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(requestId, out Guid parsedRequestId))
        {
            return BadRequest();
        }
        
        LoginUserCommand loginCommand = new(
            parsedRequestId,
            request.Email,
            request.Password);

        Result<LoginResponse> result = await _sender.Send(loginCommand, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [Authorize]
    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        RefreshSessionCommand refreshCommand = new();

        Result<RefreshSessionResponse> result = await _sender.Send(refreshCommand, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}