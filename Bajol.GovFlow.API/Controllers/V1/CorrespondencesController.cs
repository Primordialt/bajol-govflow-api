using System.Security.Claims;
using Bajol.GovFlow.API.Contracts.V1.Correspondences;
using Bajol.GovFlow.API.Models;
using Bajol.GovFlow.Application.Correspondences.Commands.CreateCorrespondence;
using Bajol.GovFlow.Application.Correspondences.Dtos;
using Bajol.GovFlow.Application.Correspondences.Queries.GetCorrespondenceById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bajol.GovFlow.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/v{version:apiVersion}/correspondences")]
public sealed class CorrespondencesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CorrespondenceDto>> Create([FromBody] CreateCorrespondenceRequest request, CancellationToken cancellationToken)
    {
        var createdBy = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("preferred_username")
            ?? User.Identity?.Name
            ?? "unknown";

        var result = await mediator.Send(new CreateCorrespondenceCommand(request.Subject, createdBy), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CorrespondenceDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCorrespondenceByIdQuery(id), cancellationToken);
        return Ok(result);
    }
}
