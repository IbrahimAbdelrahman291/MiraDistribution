using MediatR;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Domain.Entities;
using MiraDistribution.Domain.Exceptions;

namespace MiraDistribution.Application.Features.Books.SetBookReceivedDate
{
    public class SetBookReceivedDateCommandHandler : IRequestHandler<SetBookReceivedDateCommand>
    {
        private readonly IApplicationDbContext _context;

        public SetBookReceivedDateCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(SetBookReceivedDateCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(new object[] { request.BookId }, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), request.BookId);

            if (book.DeliveryDate is null)
                throw new DomainException("الدفتر ده لسه متسلمش لموزع، مينفعش تسجل تاريخ استلام.");

            if (request.ReceivedDate < book.DeliveryDate)
                throw new DomainException("تاريخ الاستلام مينفعش يكون قبل تاريخ التسليم.");

            book.SetReceivedDate(request.ReceivedDate);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}