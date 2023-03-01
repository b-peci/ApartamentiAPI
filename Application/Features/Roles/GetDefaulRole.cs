using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Roles;

public class GetDefaultRole : IRequest<Guid>
{
    
}

public class GetDefaultRoleHandler : IRequestHandler<GetDefaultRole, Guid>
{
    private readonly IRoleRepository _repository;

    public GetDefaultRoleHandler(IRoleRepository repository)
    {
        _repository = repository;
    }

    public Task<Guid> Handle(GetDefaultRole request, CancellationToken cancellationToken)
    {
        return _repository.GetDefaultRole();
    }
} 