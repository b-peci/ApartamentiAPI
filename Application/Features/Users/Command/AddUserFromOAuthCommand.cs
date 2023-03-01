using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Command;

public record AddUserFromOAuthCommand : IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}


public record AddUserFromOAuthCommandHandler : IRequestHandler<AddUserFromOAuthCommand, string>
{
    private readonly IUserRepository _repository;
    private readonly IPassword _password;

    public AddUserFromOAuthCommandHandler(IUserRepository repository, IPassword password)
    {
        _repository = repository;
        _password = password;
    }    

    public async Task<string> Handle(AddUserFromOAuthCommand request, CancellationToken cancellationToken)
    {
        try
        {
            (string password, byte[] salt) = _password.HashPassword(request.Password);
            var newUser = new User(request.Email, password, salt, request.FirstName, request.LastName,
                dateOfBirth: null, Guid.Parse("57c93f57-6a25-4c72-890d-5414689b7684"),  isFromOauth: true);
            bool result = await _repository.AddUserFromOAuth(newUser);
            return result ? "" : "Could not create user";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
      
    }
} 