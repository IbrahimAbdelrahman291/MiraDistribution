using MiraDistribution.Domain.Enums;


namespace MiraDistribution.Application.Common.Models
{
    public record BookDto(
    int Id,
    BookType Type,
    int SerialStart,
    int SerialEnd,
    BookStatus Status,
    int? DistributorId,
    string? DistributorName,
    DateTime CreatedAt);
}
