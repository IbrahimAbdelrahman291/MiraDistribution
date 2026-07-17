using MediatR;
using MiraDistribution.Domain.Enums;

namespace MiraDistribution.Application.Features.Books.ChangeBookStatus
{
    public record ChangeBookStatusCommand(int BookId, BookStatus NewStatus) : IRequest;

}
