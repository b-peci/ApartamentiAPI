using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Query;

public record GetUserEmailByIdQuery(Guid UserId) : IRequest<string>;

public record GetUserEmailByIdQueryHandler(IUserRepository _repository) : IRequestHandler<GetUserEmailByIdQuery, string>
{
    public async Task<string> Handle(GetUserEmailByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetUserEmailFromId(request.UserId);
    }
}