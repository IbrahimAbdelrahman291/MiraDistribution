using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiraDistribution.Application.Common.Models;
using MiraDistribution.Application.Features.Books.AssignBook;
using MiraDistribution.Application.Features.Books.ChangeBookStatus;
using MiraDistribution.Application.Features.Books.CreateBook;
using MiraDistribution.Application.Features.Books.DeleteBook;
using MiraDistribution.Application.Features.Books.GetBooks;
using MiraDistribution.Application.Features.Books.SearchBookByReceipt;
using MiraDistribution.Application.Features.Books.TransferBook;
using MiraDistribution.Application.Features.Books.UpdateBookSerial;
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

        [HttpPost]
        public async Task<ActionResult<BookDto>> Create(CreateBookCommand command)
            => Ok(await _mediator.Send(command));

        [HttpGet]
        public async Task<ActionResult<List<BookDto>>> GetAll([FromQuery] BookStatus? status, [FromQuery] int? distributorId)
            => Ok(await _mediator.Send(new GetBooksQuery(status, distributorId)));

        [HttpGet("search")]
        public async Task<ActionResult<BookDto>> Search([FromQuery] int receiptNumber)
            => Ok(await _mediator.Send(new SearchBookByReceiptQuery(receiptNumber)));

        [HttpPut("{id:int}/serial")]
        public async Task<IActionResult> UpdateSerial(int id, UpdateSerialRequest request)
        {
            await _mediator.Send(new UpdateBookSerialCommand(id, request.NewSerialStart));
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteBookCommand(id));
            return NoContent();
        }

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
    }

    public record UpdateSerialRequest(int NewSerialStart);
    public record AssignRequest(int DistributorId);
    public record TransferRequest(int NewDistributorId);
    public record ChangeStatusRequest(BookStatus NewStatus);
}