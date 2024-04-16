using Application.Features.UserRegistrations.Commands.Confirm;
using Application.Features.UserRegistrations.Commands.Create;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace WebAPI.Controllers;

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
    public async Task<IResult> RegisterUser(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        CreateUserRegistrationCommand registrationCommand = new(
            request.Firstname,
            request.Lastname,
            request.Email,
            request.Password,
            request.Role);

        Result<Guid> result = await _sender.Send(registrationCommand, cancellationToken);
        
        return result.IsSuccess 
            ? Results.Json(result.Value, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }
    
    [HttpPost]
    [Route("confirm-user")]
    public async Task<IResult> ConfirmUser(
        [FromBody] ConfirmUserRequest request,
        CancellationToken cancellationToken)
    {
        ConfirmUserRegistrationCommand confirmCommand = new(
            request.UserRegistrationId,
            request.ConfirmationCode);

        Result result = await _sender.Send(confirmCommand, cancellationToken);
        
        return result.IsSuccess 
            ? Results.Json(null, statusCode: (int)result.Status)
            : Results.Json(result.Error, statusCode: (int)result.Status);
    }
}