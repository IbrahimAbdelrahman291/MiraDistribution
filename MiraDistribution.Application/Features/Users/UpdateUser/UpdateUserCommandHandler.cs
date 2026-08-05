using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Domain.Enums;
using ValidationException = MiraDistribution.Application.Common.Exceptions.ValidationException;

namespace MiraDistribution.Application.Features.Users.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationDbContext _context;

        public UpdateUserCommandHandler(IIdentityService identityService, IApplicationDbContext context)
        {
            _identityService = identityService;
            _context = context;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var role = await _identityService.GetUserRoleByIdAsync(request.UserId)
                ?? throw new NotFoundException("مستخدم", request.UserId);

            if (!string.IsNullOrEmpty(request.Phone))
            {
                var (succeeded, errors) = await _identityService.UpdatePhoneAsync(request.UserId, request.Phone);
                if (!succeeded)
                    throw new ValidationException(errors.Select(e => new ValidationFailure(nameof(request.Phone), e)));
            }

            if (!string.IsNullOrEmpty(request.Password))
            {
                var (succeeded, errors) = await _identityService.ResetPasswordAsync(request.UserId, request.Password);
                if (!succeeded)
                    throw new ValidationException(errors.Select(e => new ValidationFailure(nameof(request.Password), e)));
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                var (succeeded, errors) = await _identityService.UpdateNameAsync(request.UserId, request.Name);
                if (!succeeded)
                    throw new ValidationException(errors.Select(e => new ValidationFailure(nameof(request.Name), e)));
            }

            if (role == UserRole.Distributor)
            {
                var distributor = await _context.Distributors
                    .FirstOrDefaultAsync(d => d.UserId == request.UserId, cancellationToken);

                if (distributor is not null)
                {
                    if (!string.IsNullOrEmpty(request.Name))
                        distributor.Name = request.Name;

                    if (!string.IsNullOrEmpty(request.Phone))
                        distributor.Phone = request.Phone;

                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}