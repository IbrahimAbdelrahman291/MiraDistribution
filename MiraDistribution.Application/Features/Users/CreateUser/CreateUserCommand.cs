using MediatR;
using MiraDistribution.Domain.Enums;

namespace MiraDistribution.Application.Features.Users.CreateUser
{
    public record CreateUserCommand(
    string Phone,
    string Password,
    UserRole Role,
    string? Name) : IRequest<CreateUserResponse>;

    public record CreateUserResponse(string UserId, string Phone, UserRole Role);

}
