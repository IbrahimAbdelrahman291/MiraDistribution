using FluentValidation.Results;
using MediatR;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Domain.Entities;
using MiraDistribution.Domain.Enums;
using ValidationException = MiraDistribution.Application.Common.Exceptions.ValidationException;

namespace MiraDistribution.Application.Features.Users.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationDbContext _context;

        public CreateUserCommandHandler(IIdentityService identityService, IApplicationDbContext context)
        {
            _identityService = identityService;
            _context = context;
        }

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var (succeeded, userId, errors) = await _identityService.CreateUserAsync(
                request.Phone, request.Password, request.Role, request.Name);

            if (!succeeded)
            {
                var failures = errors.Select(e => new ValidationFailure(nameof(request.Phone), e));
                throw new ValidationException(failures);
            }

            if (request.Role == UserRole.Distributor)
            {
                var distributor = new Distributor
                {
                    Name = request.Name,
                    Phone = request.Phone,
                    UserId = userId!
                };

                _context.Distributors.Add(distributor);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return new CreateUserResponse(userId!, request.Phone, request.Role, request.Name);
        }
    }
}