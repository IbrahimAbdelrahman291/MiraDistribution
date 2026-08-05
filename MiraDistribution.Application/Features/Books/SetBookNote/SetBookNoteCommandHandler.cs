using MediatR;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Domain.Entities;

namespace MiraDistribution.Application.Features.Books.SetBookNote
{
    public class SetBookNoteCommandHandler : IRequestHandler<SetBookNoteCommand>
    {
        private readonly IApplicationDbContext _context;

        public SetBookNoteCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(SetBookNoteCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(new object[] { request.BookId }, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), request.BookId);

            book.SetNote(request.Note);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}