using MediatR;
using MiraDistribution.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraDistribution.Application.Features.Auth.Login
{
    public record LoginCommand(string Phone, string Password) : IRequest<LoginResponse>;

    public record LoginResponse(string Token, string UserId, string Phone, UserRole Role);
}
