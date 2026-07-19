using MediatR;
using Microsoft.EntityFrameworkCore;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Application.Common.Models;

namespace MiraDistribution.Application.Features.Distributors.GetDistributors
{
    public record GetDistributorsQuery(int PageNumber = 1, int PageSize = 20) : IRequest<PaginatedList<DistributorDto>>;

    public class GetDistributorsQueryHandler : IRequestHandler<GetDistributorsQuery, PaginatedList<DistributorDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetDistributorsQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<PaginatedList<DistributorDto>> Handle(GetDistributorsQuery request, CancellationToken cancellationToken)
        {
            var dtoQuery = _context.Distributors
                .AsNoTracking()
                .OrderBy(d => d.Name)
                .Select(d => new DistributorDto(d.Id, d.Name, d.Phone, d.Books.Count));

            return await PaginatedList<DistributorDto>.CreateAsync(
                dtoQuery, request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}