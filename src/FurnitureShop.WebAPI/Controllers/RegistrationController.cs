using FurnitureShop.Application.Features.UserRegistrations.Commands.Confirm;
using FurnitureShop.Application.Features.UserRegistrations.Commands.Create;
using FurnitureShop.Domain.Shared;
using FurnitureShop.WebAPI.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureShop.WebAPI.Controllers;

[ApiController]
[Route("api/registration")]
public class RegistrationController : Controller
{
    private readonly ISender _sender;

    public RegistrationController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpPost]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        CreateUserRegistrationCommand registrationCommand = new(
            request.Firstname,
            request.Lastname,
            request.Email,
            request.Password);

        Result<Guid> result = await _sender.Send(registrationCommand, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [HttpPost]
    [Route("confirm-user")]
    public async Task<IActionResult> ConfirmUser(
        [FromBody] ConfirmUserRequest request,
        CancellationToken cancellationToken)
    {
        ConfirmUserRegistrationCommand confirmCommand = new(
            request.UserRegistrationId,
            request.ConfirmationCode);

        Result result = await _sender.Send(confirmCommand, cancellationToken);
        
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}