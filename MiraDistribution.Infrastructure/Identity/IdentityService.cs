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

        public async Task<(bool Succeeded, string? UserId, string[] Errors)> CreateUserAsync(
            string phone, string password, UserRole role)
        {
            var existing = await _userManager.FindByNameAsync(phone);
            if (existing is not null)
                return (false, null, new[] { "رقم التليفون ده مسجل بالفعل." });

            var user = new ApplicationUser
            {
                UserName = phone,
                PhoneNumber = phone
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return (false, null, result.Errors.Select(e => e.Description).ToArray());

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
        public async Task<List<(string UserId, string Phone, UserRole Role)>> GetAllUsersAsync()
        {
            var result = new List<(string, string, UserRole)>();
            var users = _userManager.Users.ToList(); // جدول المستخدمين مش كبير عادةً، تحميله كامل مقبول هنا

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault();

                if (roleName is not null && Enum.TryParse<UserRole>(roleName, out var role))
                {
                    result.Add((user.Id, user.PhoneNumber ?? user.UserName!, role));
                }
            }

            return result;
        }
    }
}