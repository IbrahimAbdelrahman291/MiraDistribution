using MediatR;
using Microsoft.EntityFrameworkCore;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Domain.Entities;
using MiraDistribution.Domain.Enums;
using MiraDistribution.Domain.Exceptions;

namespace MiraDistribution.Application.Features.Books.TransferBook
{
    public class TransferBookCommandHandler : IRequestHandler<TransferBookCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public TransferBookCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task Handle(TransferBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(new object[] { request.BookId }, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), request.BookId);

            if (book.DistributorId is null)
                throw new DomainException("الدفتر مش متعين لحد أصلاً، استخدم عملية الإسناد (Assign) بدل النقل.");

            if (book.DistributorId == request.NewDistributorId)
                throw new DomainException("الدفتر أصلاً مع نفس الموزع ده.");

            if (book.Status == BookStatus.FullyCollected)
                throw new DomainException("الدفتر ده مكتمل بالفعل، مينفعش يتنقل لموزع تاني.");

            var newDistributorExists = await _context.Distributors
                .AnyAsync(d => d.Id == request.NewDistributorId, cancellationToken);

            if (!newDistributorExists)
                throw new NotFoundException(nameof(Distributor), request.NewDistributorId);

            var openHistory = await _context.BookAssignmentHistories
                .Where(h => h.BookId == book.Id && h.UnassignedAt == null)
                .OrderByDescending(h => h.AssignedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (openHistory is not null)
                openHistory.UnassignedAt = DateTime.UtcNow;

            

            book.AssignTo(request.NewDistributorId);

            _context.BookAssignmentHistories.Add(new BookAssignmentHistory
            {
                BookId = book.Id,
                DistributorId = request.NewDistributorId,
                AssignedByUserId = _currentUser.UserId!,
                AssignedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}