using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Command;

public record VerifyUserCommand(Guid userId) : IRequest<string>;

public record VerifyUserCommandHandler : IRequestHandler<VerifyUserCommand, string>
{
    private readonly IUserRepository _repository;

    public VerifyUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(VerifyUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // TODO: Dont let user approve his email if it has already been approved
            await _repository.ApproveUser(request.userId);
                return "User's account has been verified";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "Could not verify user's account";
        }
        
    }
}