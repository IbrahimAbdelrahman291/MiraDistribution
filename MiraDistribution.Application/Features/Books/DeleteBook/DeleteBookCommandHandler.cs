using MediatR;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Domain.Entities;

namespace MiraDistribution.Application.Features.Books.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteBookCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(new object[] { request.BookId }, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), request.BookId);

            _context.Books.Remove(book);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
