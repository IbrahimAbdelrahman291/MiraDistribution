using MediatR;
using Microsoft.EntityFrameworkCore;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Application.Common.Models;
using MiraDistribution.Domain.Entities;
using MiraDistribution.Domain.Exceptions;

namespace MiraDistribution.Application.Features.Books.CreateBooks
{
    public class CreateBooksCommandHandler : IRequestHandler<CreateBooksCommand, List<BookDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public CreateBooksCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<List<BookDto>> Handle(CreateBooksCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId!;
            var books = new List<Book>();

            var currentSerialStart = request.FirstSerialStart;
            for (int i = 0; i < request.Count; i++)
            {
                var book = new Book(request.Type, currentSerialStart, userId);
                books.Add(book);
                currentSerialStart = book.SerialEnd + 1; // الدفتر الجاي يبدأ من بعد ما الأخير خلص
            }

            var overallStart = books.First().SerialStart;
            var overallEnd = books.Last().SerialEnd;

            // تحقق واحد شامل بدل ما نضرب الداتابيز 200 مرة (performance)
            var overlapping = await _context.Books.AnyAsync(b =>
                b.SerialStart <= overallEnd && b.SerialEnd >= overallStart, cancellationToken);

            if (overlapping)
                throw new DomainException(
                    $"النطاق الإجمالي ({overallStart} - {overallEnd}) بيتداخل مع دفتر أو أكتر موجود بالفعل. " +
                    "راجع رقم بداية السيريال.");

            _context.Books.AddRange(books);
            await _context.SaveChangesAsync(cancellationToken);

            return books.Select(b => new BookDto(
                    b.Id, b.Type, b.SerialStart, b.SerialEnd, b.Status,
                    b.DistributorId, b.Distributor != null ? b.Distributor.Name : null, b.DeliveryDate, b.ReceivedDate, b.Notes, b.CreatedAt)).ToList();
        }
    }
}