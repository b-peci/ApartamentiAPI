using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Host;

public static class ValidationFunctionality<TValidator, TRequest> where TValidator : IValidator<TRequest>
{
    public static async Task<ValidationResult> Validate(TRequest request, TValidator validation)
    {
        ValidationResult validations = await validation.ValidateAsync(request);
        return validations;
    }

    public static async Task<IResult> ExecuteRequestWithValidation(TValidator validation, TRequest request, IMediator _mediator)
    {
        var validationResult = (await Validate(request, validation));
        return validationResult.IsValid
            ? Results.Ok(await _mediator.Send(request))
            : Results.ValidationProblem(validationResult.ToDictionary());
    }
}