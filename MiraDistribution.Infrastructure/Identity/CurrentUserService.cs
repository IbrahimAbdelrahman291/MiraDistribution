using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Domain.Enums;

namespace MiraDistribution.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public UserRole? Role
        {
            get
            {
                var roleClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
                return roleClaim is not null && Enum.TryParse<UserRole>(roleClaim, out var role)
                    ? role
                    : null;
            }
        }
    }
}
