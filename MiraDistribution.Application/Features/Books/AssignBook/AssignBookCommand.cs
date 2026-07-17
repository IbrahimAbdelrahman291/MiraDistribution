using MediatR;

namespace MiraDistribution.Application.Features.Books.AssignBook
{
    public record AssignBookCommand(int BookId, int DistributorId) : IRequest;

}
