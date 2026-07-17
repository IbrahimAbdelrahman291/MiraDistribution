using MiraDistribution.Domain.Enums;

namespace MiraDistribution.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        UserRole? Role { get; }
    }
}
