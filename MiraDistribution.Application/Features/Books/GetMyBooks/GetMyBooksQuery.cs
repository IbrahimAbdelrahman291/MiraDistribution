using MediatR;
using Microsoft.EntityFrameworkCore;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Application.Common.Models;

namespace MiraDistribution.Application.Features.Books.GetMyBooks
{
    public record GetMyBooksQuery(int PageNumber = 1, int PageSize = 20) : IRequest<PaginatedList<BookDto>>;

    public class GetMyBooksQueryHandler : IRequestHandler<GetMyBooksQuery, PaginatedList<BookDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetMyBooksQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<BookDto>> Handle(GetMyBooksQuery request, CancellationToken cancellationToken)
        {
            var distributor = await _context.Distributors
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.UserId == _currentUser.UserId, cancellationToken)
                ?? throw new NotFoundException("بروفايل الموزع", _currentUser.UserId!);

            var dtoQuery = _context.Books
                .AsNoTracking()
                .Where(b => b.DistributorId == distributor.Id)
                .OrderByDescending(b => b.Id)
                .Select(b => new BookDto(
                    b.Id, b.Type, b.SerialStart, b.SerialEnd, b.Status,
                    b.DistributorId, distributor.Name, b.CreatedAt));

            return await PaginatedList<BookDto>.CreateAsync(
                dtoQuery, request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}