using Bajol.GovFlow.Application.Correspondences.Dtos;
using MediatR;

namespace Bajol.GovFlow.Application.Correspondences.Queries.GetCorrespondenceById;

public sealed record GetCorrespondenceByIdQuery(Guid Id) : IRequest<CorrespondenceDto>;
