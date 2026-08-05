using MediatR;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MiraDistribution.Application.Features.Books.BulkDeleteBooks
{
    public class BulkDeleteBooksCommandHandler : IRequestHandler<BulkDeleteBooksCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public BulkDeleteBooksCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task<int> Handle(BulkDeleteBooksCommand request, CancellationToken cancellationToken)
        {
            var books = await _context.Books
                .Where(b => request.BookIds.Contains(b.Id))
                .ToListAsync(cancellationToken);

            if (books.Count != request.BookIds.Distinct().Count())
            {
                var foundIds = books.Select(b => b.Id).ToHashSet();
                var missingIds = request.BookIds.Where(id => !foundIds.Contains(id));
                throw new NotFoundException("دفاتر", string.Join(", ", missingIds));
            }

            _context.Books.RemoveRange(books);
            await _context.SaveChangesAsync(cancellationToken);

            return books.Count;
        }
    }
}
