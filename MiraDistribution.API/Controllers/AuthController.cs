using MediatR;
using MiraDistribution.Application.Features.Auth.Login;
using Microsoft.AspNetCore.Mvc;

namespace MiraDistribution.API.Controllers;

[ApiController]
[Route("api/v1/auth")]

public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}