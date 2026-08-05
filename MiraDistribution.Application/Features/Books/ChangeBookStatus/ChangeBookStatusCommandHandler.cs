using MediatR;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Domain.Entities;
using MiraDistribution.Domain.Enums;
using MiraDistribution.Domain.Exceptions;

namespace MiraDistribution.Application.Features.Books.ChangeBookStatus
{
    public class ChangeBookStatusCommandHandler : IRequestHandler<ChangeBookStatusCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public ChangeBookStatusCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task Handle(ChangeBookStatusCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(new object[] { request.BookId }, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), request.BookId);

            var isAdmin = _currentUser.Role == UserRole.Admin;

            if (!isAdmin && book.Status == BookStatus.FullyCollected)
                throw new DomainException("الدفتر ده وصل لحالة 'مكتمل' بالفعل، مينفعش تتغيّر حالته تاني.");

            if (request.NewStatus != BookStatus.NotAssigned && book.DistributorId is null)
                throw new DomainException("مينفعش تغيّر حالة الدفتر ده وهو لسه مش متعين لأي موزع.");

            if (request.NewStatus == BookStatus.NotAssigned)
                book.Unassign();
            else
                book.Status = request.NewStatus;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}