using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiraDistribution.Application.Features.Users.CreateUser;
using MiraDistribution.Application.Features.Users.DeleteUser;
using MiraDistribution.Application.Features.Users.GetUsers;
using MiraDistribution.Application.Features.Users.UpdateUser;

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
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            await _mediator.Send(new DeleteUserCommand(userId));
            return NoContent();
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(string userId, UpdateUserRequest request)
        {
            await _mediator.Send(new UpdateUserCommand(userId, request.Phone, request.Password, request.Name));
            return NoContent();
        }
    }
    public record UpdateUserRequest(string? Phone, string? Password, string? Name);
}