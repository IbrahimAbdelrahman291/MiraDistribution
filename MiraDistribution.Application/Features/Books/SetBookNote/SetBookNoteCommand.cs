using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraDistribution.Application.Features.Books.SetBookNote
{
    public record SetBookNoteCommand(int BookId, string? Note) : IRequest;
}
