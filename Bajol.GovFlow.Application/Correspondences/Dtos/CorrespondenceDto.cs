namespace Bajol.GovFlow.Application.Correspondences.Dtos;

public sealed record CorrespondenceDto(
    Guid Id,
    string Subject,
    string ReferenceNumber,
    string Status,
    DateTime CreatedAtUtc,
    string CreatedBy);
