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

        public ChangeBookStatusCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(ChangeBookStatusCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(new object[] { request.BookId }, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), request.BookId);

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