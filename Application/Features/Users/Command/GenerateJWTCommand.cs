using Application.Interfaces;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Commands;

public class GenerateJWTCommand : IRequest<string>
{
    public string Email { get; set; }
    public string? FullName { get; set; }

    public GenerateJWTCommand(string email, string? fullName)
    {
        Email = email;
        FullName = fullName;
    }
}

public class GenerateJWTCommandHandler : IRequestHandler<GenerateJWTCommand, string>
{
    private readonly IJWTToken _token;
    private readonly IUserRepository _repository;


    public GenerateJWTCommandHandler(IJWTToken token, IUserRepository repository)
    {
        _token = token;
        _repository = repository;
    }

    public async Task<string> Handle(GenerateJWTCommand request, CancellationToken cancellationToken)
    {

        try
        {
            if(string.IsNullOrEmpty(request.FullName))
                request.FullName = await _repository.GetUserFullNameFromUsername(request.Email);
            Guid userId = await _repository.GetUserIdFromEmail(request.Email);
            string token = await _token.CreateToken(request.Email, request.FullName, userId.ToString());
            return token;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
} 