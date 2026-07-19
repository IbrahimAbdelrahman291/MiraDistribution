using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiraDistribution.Application.Features.Users.CreateUser;
using MiraDistribution.Application.Features.Users.GetUsers;

namespace MiraDistribution.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin/users")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<CreateUserResponse>> Create(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
            => Ok(await _mediator.Send(new GetUsersQuery()));
    }
}