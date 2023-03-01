using Application.Features.Posts.Commands;
using FluentValidation;

namespace Application.Validation;

public class AddPostCommandValidation : AbstractValidator<AddPostCommand>
{
    public AddPostCommandValidation()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull()
            .MinimumLength(10)
            .MaximumLength(60)
            .WithMessage("Please provide a valid title");
        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull()
            .MinimumLength(30)
            .MaximumLength(1024)
            .WithMessage("Please provide a valid description");
        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage("Please provide a valid price");

        RuleFor(x => x.Base64EncodedImages)
            .Must(x => x.Length <= 12);
    }
}