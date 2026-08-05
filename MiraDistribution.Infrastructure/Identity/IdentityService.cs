using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using MiraDistribution.Infrastructure.Identity;

namespace MiraDistribution.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(bool Succeeded, string? UserId, IEnumerable<string> Errors)> CreateUserAsync(
    string phone, string password, UserRole role, string fullName)
        {
            var user = new ApplicationUser
            {
                UserName = phone,
                PhoneNumber = phone,
                FullName = fullName
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return (false, null, result.Errors.Select(e => e.Description));

            await _userManager.AddToRoleAsync(user, role.ToString());
            return (true, user.Id, Array.Empty<string>());
        }


        public async Task<string?> GetUserIdByPhoneAsync(string phone)
        {
            var user = await _userManager.FindByNameAsync(phone);
            return user?.Id;
        }

        public async Task<bool> CheckPasswordAsync(string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return false;
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<UserRole?> GetUserRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault();

            return roleName is not null && Enum.TryParse<UserRole>(roleName, out var role)
                ? role
                : null;
        }

        public async Task<bool> AnyUserExistsWithRoleAsync(UserRole role)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.ToString());
            return usersInRole.Count > 0;
        }
        public async Task<List<(string UserId, string Phone, string FullName, UserRole Role)>> GetAllUsersAsync()
        {
            var result = new List<(string, string, string, UserRole)>();
            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault();

                if (roleName is not null && Enum.TryParse<UserRole>(roleName, out var role))
                    result.Add((user.Id, user.PhoneNumber ?? user.UserName!, user.FullName, role));
            }

            return result;
        }
        public async Task<UserRole?> GetUserRoleByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault();
            return roleName is not null && Enum.TryParse<UserRole>(roleName, out var role) ? role : null;
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> UpdatePhoneAsync(string userId, string newPhone)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return (false, new[] { "المستخدم غير موجود." });

            user.PhoneNumber = newPhone;
            user.UserName = newPhone; // بما إن اليوزرنيم = رقم التليفون
            var result = await _userManager.UpdateAsync(user);

            return (result.Succeeded, result.Errors.Select(e => e.Description));
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> ResetPasswordAsync(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return (false, new[] { "المستخدم غير موجود." });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            return (result.Succeeded, result.Errors.Select(e => e.Description));
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
                await _userManager.DeleteAsync(user);
        }
        public async Task<(bool Succeeded, IEnumerable<string> Errors)> UpdateNameAsync(string userId, string fullName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return (false, new[] { "المستخدم غير موجود." });

            user.FullName = fullName;
            var result = await _userManager.UpdateAsync(user);
            return (result.Succeeded, result.Errors.Select(e => e.Description));
        }
    }
}