using MediatR;

namespace MiraDistribution.Application.Features.Books.SetBookReceivedDate
{
    public record SetBookReceivedDateCommand(int BookId, DateTime ReceivedDate) : IRequest;
}
