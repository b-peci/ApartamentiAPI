using Application.Features.Users.Command;
using FluentValidation;
using FluentValidation.Validators;

namespace Application.Validation;

public class AddUserCommandValidation : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidation()
    {
        RuleFor(x => x.Email)
            .EmailAddress(mode: EmailValidationMode.AspNetCoreCompatible)
            .NotEmpty()
            .NotNull()
            .WithMessage("Email is not valid")
            .MaximumLength(50)
            .WithMessage("Email can not be longer than 50 characters");
        RuleFor(x => x.Password)
            .MinimumLength(8)
            .WithMessage("Password should be longer than 8 characters");
        RuleFor(x => x.FirstName)
            .NotNull()
            .WithMessage("Please provide a name");
        RuleFor(x => x.LastName)
            .NotNull()
            .WithMessage("Please provide a last name");
        RuleFor(x => x.RoleId)
            .Must(CostumValidations.IsValueDefault)
            .WithMessage("Please select a role");
    }
}