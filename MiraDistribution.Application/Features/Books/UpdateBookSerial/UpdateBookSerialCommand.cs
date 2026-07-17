using MediatR;

namespace MiraDistribution.Application.Features.Books.UpdateBookSerial
{
    public record UpdateBookSerialCommand(int BookId, int NewSerialStart) : IRequest;

}
