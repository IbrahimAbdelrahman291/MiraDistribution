using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiraDistribution.Application.Common.Models;
using MiraDistribution.Application.Features.Distributors.GetDistributors;
using MiraDistribution.Application.Features.Distributors.UpdateDistributor;

namespace MiraDistribution.API.Controllers
{
    [ApiController]
    [Route("api/v1/distributors")]
    [Authorize(Roles = "Accountant,Admin")]
    public class DistributorsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DistributorsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<PaginatedList<DistributorDto>>> GetAll(
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
            => Ok(await _mediator.Send(new GetDistributorsQuery(pageNumber, pageSize)));

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateDistributorRequest request)
        {
            await _mediator.Send(new UpdateDistributorCommand(id, request.Name, request.Phone));
            return NoContent();
        }
    }

    public record UpdateDistributorRequest(string Name, string Phone);
}