using MediatR;


namespace MiraDistribution.Application.Features.Books.DeleteBook
{
    public record DeleteBookCommand(int BookId) : IRequest;

}
