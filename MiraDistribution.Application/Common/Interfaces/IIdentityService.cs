using MiraDistribution.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraDistribution.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<(bool Succeeded, string? UserId, IEnumerable<string> Errors)> CreateUserAsync(
            string phone, string password, UserRole role, string fullName);
        Task<string?> GetUserIdByPhoneAsync(string phone);
        Task<bool> CheckPasswordAsync(string userId, string password);
        Task<UserRole?> GetUserRoleAsync(string userId);
        Task<bool> AnyUserExistsWithRoleAsync(UserRole role);
        Task<List<(string UserId, string Phone, string FullName, UserRole Role)>> GetAllUsersAsync();
        Task<UserRole?> GetUserRoleByIdAsync(string userId);
        Task<(bool Succeeded, IEnumerable<string> Errors)> UpdatePhoneAsync(string userId, string newPhone);
        Task<(bool Succeeded, IEnumerable<string> Errors)> ResetPasswordAsync(string userId, string newPassword);
        Task<(bool Succeeded, IEnumerable<string> Errors)> UpdateNameAsync(string userId, string fullName);
        Task DeleteUserAsync(string userId);

    }
}
