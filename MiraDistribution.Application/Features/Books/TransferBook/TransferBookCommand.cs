using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraDistribution.Application.Features.Books.TransferBook
{
    public record TransferBookCommand(int BookId, int NewDistributorId) : IRequest;

}
