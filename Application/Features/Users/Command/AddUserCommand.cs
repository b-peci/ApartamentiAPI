using Application.Features.Emails;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Users.Command;

public record AddUserCommand : IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Guid RoleId { get; set; }
    public bool IsFromOAUTH { get; set; }
}


public record AddUserCommandHandler : IRequestHandler<AddUserCommand, string>
{
    private readonly IUserRepository _repository;
    private readonly IPassword _passwordHelper;
    private readonly IConfiguration _conf;
    private readonly IJWTToken _token;


    public AddUserCommandHandler(IUserRepository repository, IPassword passwordHelper, IConfiguration conf, IJWTToken token)
    {
        _repository = repository;
        _passwordHelper = passwordHelper;
        _conf = conf;
        _token = token;
    }

    public async Task<string> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            string password;
            byte[] salt;
            bool isEmailBeingUsed = await _repository.IsEmailTaken(request.Email);
            if (isEmailBeingUsed)
                return "Email is already taken";
            (password, salt) = _passwordHelper.HashPassword(request.Password);
            var user = new User(request.Email.ToLower(), password, salt, request.FirstName, request.LastName, request.DateOfBirth, request.RoleId, request.IsFromOAUTH);
            await _repository.AddUser(user);
            Guid userId = await _repository.GetUserIdFromEmail(request.Email);
            //string emailVerificationResponse = Email.SentEmail(emailTo: request.Email, emailFrom: _conf["EmailData:Email"], password: _conf["EmailData:Password"], userId);
            return string.Empty;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }
} 