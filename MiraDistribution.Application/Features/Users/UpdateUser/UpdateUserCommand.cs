using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraDistribution.Application.Features.Users.UpdateUser
{
    public record UpdateUserCommand(string UserId, string? Phone, string? Password, string? Name) : IRequest;
}
