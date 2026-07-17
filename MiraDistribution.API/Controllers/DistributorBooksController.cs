using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiraDistribution.Application.Common.Models;
using MiraDistribution.Application.Features.Books.GetMyBooks;

namespace MiraDistribution.API.Controllers;

[ApiController]
[Route("api/v1/distributor/books")]
[Authorize(Roles = "Distributor")]
public class DistributorBooksController : ControllerBase
{
    private readonly IMediator _mediator;
    public DistributorBooksController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetMyBooks()
        => Ok(await _mediator.Send(new GetMyBooksQuery()));
}