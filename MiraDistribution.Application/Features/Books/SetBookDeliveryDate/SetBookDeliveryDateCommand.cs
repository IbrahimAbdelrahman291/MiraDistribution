using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraDistribution.Application.Features.Books.SetBookDeliveryDate
{
    public record SetBookDeliveryDateCommand(int BookId, DateTime DeliveryDate) : IRequest;
}
