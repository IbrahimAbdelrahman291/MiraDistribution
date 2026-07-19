using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}