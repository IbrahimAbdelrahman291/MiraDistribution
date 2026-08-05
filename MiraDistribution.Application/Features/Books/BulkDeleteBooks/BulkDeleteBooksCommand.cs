using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraDistribution.Application.Features.Books.BulkDeleteBooks
{
    public record BulkDeleteBooksCommand(List<int> BookIds) : IRequest<int>; // بيرجع عدد الدفاتر اللي اتمسحت
}
