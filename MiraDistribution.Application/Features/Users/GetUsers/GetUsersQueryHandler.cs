using MediatR;
using Microsoft.EntityFrameworkCore;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Domain.Enums;

namespace MiraDistribution.Application.Features.Users.GetUsers
{
    public record GetUsersQuery : IRequest<List<UserDto>>;

    public record UserDto(string UserId, string Phone, UserRole Role, string? DistributorName);

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationDbContext _context;

        public GetUsersQueryHandler(IIdentityService identityService, IApplicationDbContext context)
        {
            _identityService = identityService;
            _context = context;
        }

        public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _identityService.GetAllUsersAsync();

            var distributorNames = await _context.Distributors
                .AsNoTracking()
                .Select(d => new { d.UserId, d.Name })
                .ToDictionaryAsync(d => d.UserId, d => d.Name, cancellationToken);

            return users.Select(u => new UserDto(
                u.UserId,
                u.Phone,
                u.Role,
                u.Role == UserRole.Distributor && distributorNames.TryGetValue(u.UserId, out var name) ? name : null
            )).ToList();
        }
    }
}