using MediatR;
using MiraDistribution.Application.Common.Models;
using MiraDistribution.Domain.Enums;

namespace MiraDistribution.Application.Features.Books.CreateBooks
{
    public record CreateBooksCommand(BookType Type, int FirstSerialStart, int Count) : IRequest<List<BookDto>>;

}
