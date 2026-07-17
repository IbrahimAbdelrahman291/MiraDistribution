using MediatR;
using Microsoft.EntityFrameworkCore;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Application.Common.Models;
using MiraDistribution.Domain.Entities;
using MiraDistribution.Domain.Exceptions;

namespace MiraDistribution.Application.Features.Books.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public CreateBookCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var book = new Book(request.Type, request.SerialStart, _currentUser.UserId!);

            var overlapping = await _context.Books.AnyAsync(b =>
                b.SerialStart <= book.SerialEnd && b.SerialEnd >= book.SerialStart, cancellationToken);

            if (overlapping)
                throw new DomainException(
                    $"النطاق ({book.SerialStart} - {book.SerialEnd}) بيتداخل مع دفتر تاني موجود بالفعل.");

            _context.Books.Add(book);
            await _context.SaveChangesAsync(cancellationToken);

            return new BookDto(book.Id, book.Type, book.SerialStart, book.SerialEnd, book.Status, null, null, book.CreatedAt);
        }
    }
}