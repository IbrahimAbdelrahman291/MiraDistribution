using MediatR;
using MiraDistribution.Application.Common.Exceptions;
using MiraDistribution.Application.Common.Interfaces;
using MiraDistribution.Domain.Entities;

namespace MiraDistribution.Application.Features.Distributors.UpdateDistributor
{
    public class UpdateDistributorCommandHandler : IRequestHandler<UpdateDistributorCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateDistributorCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(UpdateDistributorCommand request, CancellationToken cancellationToken)
        {
            var distributor = await _context.Distributors.FindAsync(new object[] { request.Id }, cancellationToken)
                ?? throw new NotFoundException(nameof(Distributor), request.Id);

            distributor.Name = request.Name;
            distributor.Phone = request.Phone;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
