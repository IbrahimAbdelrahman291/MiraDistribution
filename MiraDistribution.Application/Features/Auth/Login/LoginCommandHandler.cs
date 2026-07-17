using MediatR;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Application.Features.Auth.Login;

namespace MiraDistribution.Application.Features.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtGenerator _jwtGenerator;

        public LoginCommandHandler(IIdentityService identityService, IJwtGenerator jwtGenerator)
        {
            _identityService = identityService;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var userId = await _identityService.GetUserIdByPhoneAsync(request.Phone);
            if (userId is null)
                throw new AuthenticationException();

            var passwordValid = await _identityService.CheckPasswordAsync(userId, request.Password);
            if (!passwordValid)
                throw new AuthenticationException();

            var role = await _identityService.GetUserRoleAsync(userId);
            if (role is null)
                throw new AuthenticationException();

            var token = _jwtGenerator.GenerateToken(userId, request.Phone, role.Value);

            return new LoginResponse(token, userId, request.Phone, role.Value);
        }
    }
}