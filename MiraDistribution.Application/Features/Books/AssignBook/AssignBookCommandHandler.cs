using MediatR;
using Microsoft.EntityFrameworkCore;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Domain.Entities;
using MiraDistribution.Domain.Exceptions;

namespace MiraDistribution.Application.Features.Books.AssignBook
{
    public class AssignBookCommandHandler : IRequestHandler<AssignBookCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public AssignBookCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task Handle(AssignBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(new object[] { request.BookId }, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), request.BookId);

            if (book.DistributorId is not null)
                throw new DomainException("الدفتر ده متعين لموزع بالفعل، استخدم عملية النقل (Transfer) بدل الإسناد.");

            var distributorExists = await _context.Distributors
                .AnyAsync(d => d.Id == request.DistributorId, cancellationToken);

            if (!distributorExists)
                throw new NotFoundException(nameof(Distributor), request.DistributorId);

            book.AssignTo(request.DistributorId);

            _context.BookAssignmentHistories.Add(new BookAssignmentHistory
            {
                BookId = book.Id,
                DistributorId = request.DistributorId,
                AssignedByUserId = _currentUser.UserId!,
                AssignedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}