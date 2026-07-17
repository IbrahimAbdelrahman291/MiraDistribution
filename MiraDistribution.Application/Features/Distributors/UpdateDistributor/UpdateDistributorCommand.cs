using MediatR;

namespace MiraDistribution.Application.Features.Distributors.UpdateDistributor
{
    public record UpdateDistributorCommand(int Id, string Name, string Phone) : IRequest;
}
