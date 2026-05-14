using Bajol.GovFlow.Application.Correspondences.Dtos;
using MediatR;

namespace Bajol.GovFlow.Application.Correspondences.Commands.CreateCorrespondence;

public sealed record CreateCorrespondenceCommand(string Subject, string CreatedBy) : IRequest<CorrespondenceDto>;
