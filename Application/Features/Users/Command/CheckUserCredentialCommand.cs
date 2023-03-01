using Application.Features.Users.Commands;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Command;

public class CheckUserCredentialCommand : IRequest<bool>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class CheckUserCredentialCommandHandler : IRequestHandler<CheckUserCredentialCommand,bool>
{
    private readonly IUserRepository _repository;
    private readonly IPassword _passwordHelper;

    public CheckUserCredentialCommandHandler(IUserRepository repository, IPassword passwordHelper)
    {
        _repository = repository;
        _passwordHelper = passwordHelper;
        }

    public async Task<bool> Handle(CheckUserCredentialCommand request, CancellationToken cancellationToken)
    {
        byte[] userSalt = await _repository.GetSaltForUser(request.Email);
        string hashedPassword;
        (hashedPassword, userSalt) = _passwordHelper.HashPassword(request.Password, userSalt);
        bool isCorrect = await _repository.CheckCredentials(request.Email.ToLower(), hashedPassword);
        return  isCorrect;
    }
} 