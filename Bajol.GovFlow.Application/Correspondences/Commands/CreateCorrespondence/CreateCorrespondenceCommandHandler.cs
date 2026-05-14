using Bajol.GovFlow.Application.Correspondences.Dtos;
using Bajol.GovFlow.Application.Search;
using Bajol.GovFlow.Domain.Correspondences;
using Bajol.GovFlow.Domain.Repositories;
using Bajol.GovFlow.Domain.UnitOfWork;
using MediatR;
using System.Text.Json;

namespace Bajol.GovFlow.Application.Correspondences.Commands.CreateCorrespondence;

public sealed class CreateCorrespondenceCommandHandler(
    ICorrespondenceRepository correspondenceRepository,
    IUnitOfWork unitOfWork,
    ISearchGateway searchGateway,
    TimeProvider timeProvider)
    : IRequestHandler<CreateCorrespondenceCommand, CorrespondenceDto>
{
    public async Task<CorrespondenceDto> Handle(CreateCorrespondenceCommand request, CancellationToken cancellationToken)
    {
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;
        var referenceNumber = $"GOV-{utcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";

        var entity = Correspondence.Create(request.Subject, referenceNumber, request.CreatedBy, utcNow);
        correspondenceRepository.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var json = JsonSerializer.Serialize(new
        {
            entity.Id,
            entity.Subject,
            entity.ReferenceNumber,
            entity.Status,
            entity.CreatedAtUtc,
            entity.CreatedBy
        });

        await searchGateway.IndexJsonDocumentAsync("correspondences", entity.Id.ToString("D"), json, cancellationToken);

        return new CorrespondenceDto(
            entity.Id,
            entity.Subject,
            entity.ReferenceNumber,
            entity.Status.ToString(),
            entity.CreatedAtUtc,
            entity.CreatedBy);
    }
}
