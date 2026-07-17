using MediatR;
using Microsoft.EntityFrameworkCore;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Application.Common.Models;
using MiraDistribution.Domain.Entities;

namespace MiraDistribution.Application.Features.Books.GetMyBooks
{
    public record GetMyBooksQuery : IRequest<List<BookDto>>;

    public class GetMyBooksQueryHandler : IRequestHandler<GetMyBooksQuery, List<BookDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetMyBooksQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<List<BookDto>> Handle(GetMyBooksQuery request, CancellationToken cancellationToken)
        {
            var distributor = await _context.Distributors
                .FirstOrDefaultAsync(d => d.UserId == _currentUser.UserId, cancellationToken)
                ?? throw new NotFoundException("بروفايل الموزع", _currentUser.UserId!);

            return await _context.Books
                .Where(b => b.DistributorId == distributor.Id)
                .OrderByDescending(b => b.Id)   // <-- هنا قبل الـ Select
                .Select(b => new BookDto(
                    b.Id, b.Type, b.SerialStart, b.SerialEnd, b.Status,
                    b.DistributorId, distributor.Name, b.CreatedAt))
                .ToListAsync(cancellationToken);
        }
    }
}