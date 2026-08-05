using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiraDistribution.Application.Common.Models;
using MiraDistribution.Application.Features.Books.AssignBook;
using MiraDistribution.Application.Features.Books.ChangeBookStatus;
using MiraDistribution.Application.Features.Books.CreateBooks;
using MiraDistribution.Application.Features.Books.GetBooks;
using MiraDistribution.Application.Features.Books.SearchBookByReceipt;
using MiraDistribution.Application.Features.Books.SetBookDeliveryDate;
using MiraDistribution.Application.Features.Books.SetBookNote;
using MiraDistribution.Application.Features.Books.SetBookReceivedDate;
using MiraDistribution.Application.Features.Books.TransferBook;
using MiraDistribution.Domain.Enums;

namespace MiraDistribution.API.Controllers
{
    [ApiController]
    [Route("api/v1/books")]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BooksController(IMediator mediator) => _mediator = mediator;

        
        [HttpGet]
        [Authorize(Roles = "Accountant,Admin")]
        public async Task<ActionResult<PaginatedList<BookDto>>> GetAll(
            [FromQuery] BookType? type,
            [FromQuery] BookStatus? status,
            [FromQuery] int? distributorId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
            => Ok(await _mediator.Send(new GetBooksQuery(type, status, distributorId, pageNumber, pageSize)));

        [HttpGet("search")]
        [Authorize(Roles = "Accountant,Admin")]
        public async Task<ActionResult<BookDto>> Search([FromQuery] int receiptNumber)
            => Ok(await _mediator.Send(new SearchBookByReceiptQuery(receiptNumber)));

        [HttpPost("{id:int}/assign")]
        [Authorize(Roles = "Accountant,Admin")]
        public async Task<IActionResult> Assign(int id, AssignRequest request)
        {
            await _mediator.Send(new AssignBookCommand(id, request.DistributorId));
            return NoContent();
        }

        [HttpPost("{id:int}/transfer")]
        [Authorize(Roles = "Accountant,Admin")]
        public async Task<IActionResult> Transfer(int id, TransferRequest request)
        {
            await _mediator.Send(new TransferBookCommand(id, request.NewDistributorId));
            return NoContent();
        }

        [HttpPut("{id:int}/status")]
        [Authorize(Roles = $"Accountant,Admin")]
        public async Task<IActionResult> ChangeStatus(int id, ChangeStatusRequest request)
        {
            await _mediator.Send(new ChangeBookStatusCommand(id, request.NewStatus));
            return NoContent();
        }
        [HttpPost("bulk")]
        [Authorize(Roles = "Accountant,Admin")]
        public async Task<ActionResult<List<BookDto>>> CreateBulk(CreateBooksCommand command)
            => Ok(await _mediator.Send(command));

        [HttpPut("{id:int}/received-date")]
        [Authorize(Roles = "Accountant,Admin")]
        public async Task<IActionResult> SetReceivedDate(int id, SetReceivedDateRequest request)
        {
            await _mediator.Send(new SetBookReceivedDateCommand(id, request.ReceivedDate));
            return NoContent();
        }
        [HttpPut("{id:int}/note")]
        [Authorize(Roles = "Accountant,Admin")]
        public async Task<IActionResult> SetNote(int id, SetNoteRequest request)
        {
            await _mediator.Send(new SetBookNoteCommand(id, request.Note));
            return NoContent();
        }
        [HttpPut("{id:int}/delivery-date")]
        [Authorize(Roles = "Accountant,Admin")]
        public async Task<IActionResult> SetDeliveryDate(int id, SetDeliveryDateRequest request)
        {
            await _mediator.Send(new SetBookDeliveryDateCommand(id, request.DeliveryDate));
            return NoContent();
        }
    }
    public record SetDeliveryDateRequest(DateTime DeliveryDate);
    public record SetNoteRequest(string? Note);
    public record SetReceivedDateRequest(DateTime ReceivedDate);
    public record AssignRequest(int DistributorId);
    public record TransferRequest(int NewDistributorId);
    public record ChangeStatusRequest(BookStatus NewStatus);
}