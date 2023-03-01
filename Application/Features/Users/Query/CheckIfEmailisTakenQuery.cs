using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Query;

public class CheckIfEmailIsTakenQuery : IRequest<bool>
{
    public string Email { get; set; }
}


public class CheckIfEmailIsTakenQueryHandler : IRequestHandler<CheckIfEmailIsTakenQuery, bool>
{
    private readonly IUserRepository _repository;

    public CheckIfEmailIsTakenQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public Task<bool> Handle(CheckIfEmailIsTakenQuery request, CancellationToken cancellationToken)
    {
        return _repository.IsEmailTaken(request.Email);
    }
}
 