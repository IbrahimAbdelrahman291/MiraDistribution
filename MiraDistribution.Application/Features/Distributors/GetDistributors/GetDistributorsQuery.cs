using MediatR;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace MiraDistribution.Application.Features.Distributors.GetDistributors
{
    public record GetDistributorsQuery : IRequest<List<DistributorDto>>;

    public class GetDistributorsQueryHandler : IRequestHandler<GetDistributorsQuery, List<DistributorDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetDistributorsQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<List<DistributorDto>> Handle(GetDistributorsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Distributors
                .Select(d => new DistributorDto(d.Id, d.Name, d.Phone, d.Books.Count))
                .ToListAsync(cancellationToken);
        }
    }
}