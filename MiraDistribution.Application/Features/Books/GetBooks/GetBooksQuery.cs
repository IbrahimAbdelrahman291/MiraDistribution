using MediatR;
using Microsoft.EntityFrameworkCore;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Application.Common.Models;
using MiraDistribution.Domain.Enums;

namespace MiraDistribution.Application.Features.Books.GetBooks
{
    public record GetBooksQuery(BookStatus? Status, int? DistributorId) : IRequest<List<BookDto>>;

    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, List<BookDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetBooksQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<List<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Books.AsQueryable();

            if (request.Status is not null)
                query = query.Where(b => b.Status == request.Status);

            if (request.DistributorId is not null)
                query = query.Where(b => b.DistributorId == request.DistributorId);

            return await query
                .OrderByDescending(b => b.Id)   // <-- الترتيب هنا قبل الـ Select
                .Select(b => new BookDto(
                    b.Id, b.Type, b.SerialStart, b.SerialEnd, b.Status,
                    b.DistributorId, b.Distributor != null ? b.Distributor.Name : null, b.CreatedAt))
                .ToListAsync(cancellationToken);
        }
    }
}