using MediatR;
using Microsoft.EntityFrameworkCore;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Domain.Enums;
using MiraDistribution.Domain.Exceptions;

namespace MiraDistribution.Application.Features.Users.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationDbContext _context;

        public DeleteUserCommandHandler(IIdentityService identityService, IApplicationDbContext context)
        {
            _identityService = identityService;
            _context = context;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var role = await _identityService.GetUserRoleByIdAsync(request.UserId)
                ?? throw new NotFoundException("مستخدم", request.UserId);

            if (role == UserRole.Admin)
            {
                var admins = (await _identityService.GetAllUsersAsync())
                    .Count(u => u.Role == UserRole.Admin);

                if (admins <= 1)
                    throw new DomainException("مينفعش تمسح آخر حساب أدمن في النظام.");
            }

            if (role == UserRole.Distributor)
            {
                var distributor = await _context.Distributors
                    .FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);

                if (distributor is not null)
                {
                    var hasActiveBooks = await _context.Books
                        .AnyAsync(b => b.DistributorId == distributor.Id, cancellationToken);

                    if (hasActiveBooks)
                        throw new DomainException("الموزع ده لسه معاه دفاتر متعينة، لازم تنقلها لموزع تاني الأول قبل الحذف.");

                    _context.Distributors.Remove(distributor);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            await _identityService.DeleteUserAsync(request.UserId);
        }
    }
}