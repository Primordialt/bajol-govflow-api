using FluentValidation;

namespace Bajol.GovFlow.Application.Correspondences.Commands.CreateCorrespondence;

public sealed class CreateCorrespondenceCommandValidator : AbstractValidator<CreateCorrespondenceCommand>
{
    public CreateCorrespondenceCommandValidator()
    {
        RuleFor(x => x.Subject)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.CreatedBy)
            .NotEmpty()
            .MaximumLength(256);
    }
}
