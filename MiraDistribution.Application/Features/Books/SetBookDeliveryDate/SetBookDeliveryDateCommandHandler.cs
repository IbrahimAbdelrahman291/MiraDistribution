using MediatR;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Domain.Entities;
using MiraDistribution.Domain.Exceptions;

namespace MiraDistribution.Application.Features.Books.SetBookDeliveryDate
{
    public class SetBookDeliveryDateCommandHandler : IRequestHandler<SetBookDeliveryDateCommand>
    {
        private readonly IApplicationDbContext _context;

        public SetBookDeliveryDateCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(SetBookDeliveryDateCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(new object[] { request.BookId }, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), request.BookId);

            if (book.DistributorId is null)
                throw new DomainException("الدفتر لسه مش متعين لأي موزع، مينفعش تسجل تاريخ تسليم.");

            book.SetDeliveryDate(request.DeliveryDate);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}