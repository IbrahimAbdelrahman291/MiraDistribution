using MediatR;
using Microsoft.EntityFrameworkCore;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Application.Common.Models;
using MiraDistribution.Domain.Entities;

namespace MiraDistribution.Application.Features.Books.SearchBookByReceipt
{
    public record SearchBookByReceiptQuery(int ReceiptNumber) : IRequest<BookDto>;

    public class SearchBookByReceiptQueryHandler : IRequestHandler<SearchBookByReceiptQuery, BookDto>
    {
        private readonly IApplicationDbContext _context;

        public SearchBookByReceiptQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<BookDto> Handle(SearchBookByReceiptQuery request, CancellationToken cancellationToken)
        {
            var book = await _context.Books
                .Include(b => b.Distributor)
                .FirstOrDefaultAsync(b =>
                    request.ReceiptNumber >= b.SerialStart && request.ReceiptNumber <= b.SerialEnd,
                    cancellationToken)
                ?? throw new NotFoundException("إيصال", request.ReceiptNumber);

            return new BookDto(
                    book.Id, book.Type, book.SerialStart, book.SerialEnd, book.Status,
                    book.DistributorId, book.Distributor != null ? book.Distributor.Name : null, book.DeliveryDate, book.ReceivedDate, book.Notes, book.CreatedAt);
        }
    }
}