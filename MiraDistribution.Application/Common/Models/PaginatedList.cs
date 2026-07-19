using Microsoft.EntityFrameworkCore;

namespace MiraDistribution.Application.Common.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            // حد أقصى وأدنى لحماية السيرفر من طلبات غريبة (pageSize=99999 مثلاً)
            pageSize = Math.Clamp(pageSize, 1, 100);
            pageNumber = Math.Max(pageNumber, 1);

            var count = await source.CountAsync(cancellationToken);
            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }
}