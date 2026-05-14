using Bajol.GovFlow.Application.Common.Exceptions;
using Bajol.GovFlow.Application.Correspondences.Dtos;
using Bajol.GovFlow.Domain.Correspondences;
using Bajol.GovFlow.Domain.Repositories;
using MediatR;

namespace Bajol.GovFlow.Application.Correspondences.Queries.GetCorrespondenceById;

public sealed class GetCorrespondenceByIdQueryHandler(ICorrespondenceRepository correspondenceRepository)
    : IRequestHandler<GetCorrespondenceByIdQuery, CorrespondenceDto>
{
    public async Task<CorrespondenceDto> Handle(GetCorrespondenceByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await correspondenceRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            throw new NotFoundException($"Correspondence {request.Id} was not found.", "correspondence_not_found");
        }

        return Map(entity);
    }

    private static CorrespondenceDto Map(Correspondence entity) =>
        new(
            entity.Id,
            entity.Subject,
            entity.ReferenceNumber,
            entity.Status.ToString(),
            entity.CreatedAtUtc,
            entity.CreatedBy);
}
