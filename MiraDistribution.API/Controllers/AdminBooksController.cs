using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiraDistribution.Application.Features.Books.BulkDeleteBooks;
using MiraDistribution.Application.Features.Books.DeleteBook;

namespace MiraDistribution.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin/books")]
    [Authorize(Roles = "Admin")]
    public class AdminBooksController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminBooksController(IMediator mediator) => _mediator = mediator;

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteBookCommand(id));
            return NoContent();
        }
        [HttpPost("bulk-delete")]
        public async Task<ActionResult<int>> BulkDelete(BulkDeleteBooksCommand command)
        {
            var deletedCount = await _mediator.Send(command);
            return Ok(new { deletedCount });
        }
    }
}