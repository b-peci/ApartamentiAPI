using Application.Features.Roles;
using Application.Features.Users.Command;
using Application.Features.Users.Commands;
using Application.Features.Users.Query;
using Application.GlobalDtos;
using FluentValidation;
using Host.HelperMethods;
using MediatR;

namespace Host.EndPointMethods;

public static class UserEndPoint
{
    public static async Task<IResult> CheckUserCredentials(IMediator mediator, CheckUserCredentialCommand command)
    {
        try
        {
            bool isCorrect = await mediator.Send(command);
            if (!isCorrect) return Results.Ok("Credentials are not correct");
            string tokenGenerator = await GenerateToken(command.Email, fullName: string.Empty, mediator);
            return Results.Ok(tokenGenerator);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }
    
    public static async Task<IResult> AddUser(IMediator mediator, AddUserCommand command, IValidator<AddUserCommand> validation ){
        try
        {
            command.RoleId = await mediator.Send(new GetDefaultRole()); 
            return await 
                ValidationFunctionality<IValidator<AddUserCommand>, AddUserCommand>.ExecuteRequestWithValidation(validation, command, mediator);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem(e.Message);
        }
    }
    
    public static async Task<IResult> UpdateUserBasicInfo(IMediator mediator, UpdateUserBasicInfoCommand command, IValidator<UpdateUserBasicInfoCommand> validation ){
        try
        {
            var validationResult = await ValidationFunctionality<IValidator<UpdateUserBasicInfoCommand>, UpdateUserBasicInfoCommand>.Validate(command, validation);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());
            GlobalResponseDto<string> updateUserBasic = await mediator.Send(command);
            if (!updateUserBasic.IsSuccess) return Results.Problem("Could not update information");
            string userEmail = await GetEmailByUserId(command.UserId, mediator);
            string generatedToken = await GenerateToken(userEmail, command.FirstName + " " + command.LastName, mediator);
            return Results.Ok(generatedToken);
        }
        catch (Exception e)
        {
            return Results. Problem(e.Message);
        }
    }

    private static async Task<string> GenerateToken(string email, string fullName, IMediator mediator)
    {
        string token = await mediator.Send(new GenerateJWTCommand(email, fullName));
        GenerateTokenCookie(token);
        return "Token Generated";
    }

    private static async Task<string> GetEmailByUserId(Guid userId, IMediator mediator)
    {
        string email = await mediator.Send(new GetUserEmailByIdQuery(userId));
        return email;
    }




    private static void GenerateTokenCookie(string token)
    {
        new HttpContextAccessor()?.HttpContext?.Response.Cookies.Append("X-Access-Token", token, new CookieOptions()
        {
            Expires = DateTimeOffset.Now.AddDays(1),
            HttpOnly = true,
            Path = "/",
            SameSite = SameSiteMode.None,
            Secure = true
        });
    }

    

    public static async Task<IResult> AddUserFromOAuth(IMediator _mediator, AddUserFromOAuthCommand command)
    {
        try
        {
            return Results.Ok(await _mediator.Send(command));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem(e.Message);
        }
    }
    
    public static async Task<IResult> IsEmailTaken(IMediator _mediator, string email)
    {
        try
        {
            return Results.Ok(await _mediator.Send(new CheckIfEmailIsTakenQuery {Email = email}));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem(e.Message);
        }
    }
    
    public static async Task<IResult> BasicInfo()
    {
        try
        {
            Guid userId = ClaimFunctionality.GetUserIdFromClaim();
            string? fullName = ClaimFunctionality.GetFullNameFromClaims();
            return Results.Ok(new
            {
                User = userId,
                FullName = fullName
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem(e.Message);
        }
    }

    public static async Task<IResult> VerifyEmail(IMediator _mediator, VerifyUserCommand command)
    {
        try
        {
            return Results.Ok(await _mediator.Send(command));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem(e.Message);
        }
    }
}