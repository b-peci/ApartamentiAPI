using Application.Features.Users.Command;
using FluentValidation;

namespace Application.Validation;

public class UpdateUserBasicInfoValidation : AbstractValidator<UpdateUserBasicInfoCommand>
{
    public UpdateUserBasicInfoValidation()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .NotNull()
            .MaximumLength(50)
            .WithMessage("Please provide a name");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .NotNull()
            .MaximumLength(50)
            .WithMessage("Please provide a last name");
    }
}