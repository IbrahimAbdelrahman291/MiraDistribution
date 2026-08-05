using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraDistribution.Application.Features.Users.DeleteUser
{
    public record DeleteUserCommand(string UserId) : IRequest;
}
