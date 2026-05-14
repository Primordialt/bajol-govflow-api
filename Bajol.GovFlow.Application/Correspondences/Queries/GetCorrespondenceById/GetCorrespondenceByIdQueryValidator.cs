using FluentValidation;

namespace Bajol.GovFlow.Application.Correspondences.Queries.GetCorrespondenceById;

public sealed class GetCorrespondenceByIdQueryValidator : AbstractValidator<GetCorrespondenceByIdQuery>
{
    public GetCorrespondenceByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
