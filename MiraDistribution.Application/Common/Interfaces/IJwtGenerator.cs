using MiraDistribution.Domain.Enums;


namespace MiraDistribution.Application.Common.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateToken(string userId, string phone, UserRole role);
    }
}
