using MediatR;
using Microsoft.EntityFrameworkCore;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Application.Common.Models;
using MiraDistribution.Domain.Enums;

namespace MiraDistribution.Application.Features.Books.GetBooks
{
    public record GetBooksQuery(
    BookType? Type,
    BookStatus? Status,
    int? DistributorId,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<PaginatedList<BookDto>>;

    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, PaginatedList<BookDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetBooksQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<PaginatedList<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Books.AsNoTracking().AsQueryable();

            if (request.Type is not null)
                query = query.Where(b => b.Type == request.Type);

            if (request.Status is not null)
                query = query.Where(b => b.Status == request.Status);

            if (request.DistributorId is not null)
                query = query.Where(b => b.DistributorId == request.DistributorId);

            var dtoQuery = query
                .OrderBy(b => b.Id)
                .Select(b => new BookDto(
                    b.Id, b.Type, b.SerialStart, b.SerialEnd, b.Status,
                    b.DistributorId, b.Distributor != null ? b.Distributor.Name : null,b.DeliveryDate,b.ReceivedDate,b.Notes, b.CreatedAt));

            return await PaginatedList<BookDto>.CreateAsync(
                dtoQuery, request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}