using MiraDistribution.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraDistribution.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<(bool Succeeded, string? UserId, string[] Errors)> CreateUserAsync(string phone, string password, UserRole role);
        Task<string?> GetUserIdByPhoneAsync(string phone);
        Task<bool> CheckPasswordAsync(string userId, string password);
        Task<UserRole?> GetUserRoleAsync(string userId);
        Task<bool> AnyUserExistsWithRoleAsync(UserRole role);
        Task<List<(string UserId, string Phone, UserRole Role)>> GetAllUsersAsync();

    }
}
