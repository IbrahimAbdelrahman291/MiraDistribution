using MediatR;
using MiraDistribution.Application.Common.Models;
using MiraDistribution.Domain.Enums;

namespace MiraDistribution.Application.Features.Books.CreateBook
{
    public record CreateBookCommand(BookType Type, int SerialStart) : IRequest<BookDto>;
}
