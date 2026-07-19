using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiraDistribution.Application.Common.Models;
using MiraDistribution.Application.Features.Books.AssignBook;
using MiraDistribution.Application.Features.Books.ChangeBookStatus;
using MiraDistribution.Application.Features.Books.CreateBooks;
using MiraDistribution.Application.Features.Books.DeleteBook;
using MiraDistribution.Application.Features.Books.GetBooks;
using MiraDistribution.Application.Features.Books.SearchBookByReceipt;
using MiraDistribution.Application.Features.Books.TransferBook;
using MiraDistribution.Domain.Enums;

namespace MiraDistribution.API.Controllers
{
    [ApiController]
    [Route("api/v1/books")]
    [Authorize(Roles = "Accountant")]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BooksController(IMediator mediator) => _mediator = mediator;

        
        [HttpGet]
        public async Task<ActionResult<PaginatedList<BookDto>>> GetAll(
            [FromQuery] BookType? type,
            [FromQuery] BookStatus? status,
            [FromQuery] int? distributorId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
            => Ok(await _mediator.Send(new GetBooksQuery(type, status, distributorId, pageNumber, pageSize)));

        [HttpGet("search")]
        public async Task<ActionResult<BookDto>> Search([FromQuery] int receiptNumber)
            => Ok(await _mediator.Send(new SearchBookByReceiptQuery(receiptNumber)));

        [HttpPost("{id:int}/assign")]
        public async Task<IActionResult> Assign(int id, AssignRequest request)
        {
            await _mediator.Send(new AssignBookCommand(id, request.DistributorId));
            return NoContent();
        }

        [HttpPost("{id:int}/transfer")]
        public async Task<IActionResult> Transfer(int id, TransferRequest request)
        {
            await _mediator.Send(new TransferBookCommand(id, request.NewDistributorId));
            return NoContent();
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> ChangeStatus(int id, ChangeStatusRequest request)
        {
            await _mediator.Send(new ChangeBookStatusCommand(id, request.NewStatus));
            return NoContent();
        }
        [HttpPost("bulk")]
        public async Task<ActionResult<List<BookDto>>> CreateBulk(CreateBooksCommand command)
            => Ok(await _mediator.Send(command));
    }

    public record AssignRequest(int DistributorId);
    public record TransferRequest(int NewDistributorId);
    public record ChangeStatusRequest(BookStatus NewStatus);
}