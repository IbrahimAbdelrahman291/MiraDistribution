using MediatR;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Domain.Entities;
using MiraDistribution.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MiraDistribution.Application.Features.Books.UpdateBookSerial
{
    public class UpdateBookSerialCommandHandler : IRequestHandler<UpdateBookSerialCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateBookSerialCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(UpdateBookSerialCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(new object[] { request.BookId }, cancellationToken)
                ?? throw new NotFoundException(nameof(Book), request.BookId);

            var newEnd = request.NewSerialStart + 49;

            var overlapping = await _context.Books.AnyAsync(b =>
                b.Id != book.Id && b.SerialStart <= newEnd && b.SerialEnd >= request.NewSerialStart,
                cancellationToken);

            if (overlapping)
                throw new DomainException(
                    $"النطاق ({request.NewSerialStart} - {newEnd}) بيتداخل مع دفتر تاني موجود بالفعل.");

            book.SetSerialStart(request.NewSerialStart);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
