using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Posts.Commands;

public record UpdatePostTimeInteractedCommand(List<Guid> PostIds, Guid UserId) : IRequest<string>;


public record UpdatePostTimeInteractedCommandHandler(IPostRepository _repository) : IRequestHandler<UpdatePostTimeInteractedCommand, string>
{
    public async Task<string> Handle(UpdatePostTimeInteractedCommand request, CancellationToken cancellationToken)
    {
        await _repository.AddPostTimeInteracted(request.PostIds, request.UserId);
        return "Updated";
    }
}