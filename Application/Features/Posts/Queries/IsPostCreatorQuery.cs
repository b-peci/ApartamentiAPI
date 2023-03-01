using Application.Interfaces.Repositories;
using MediatR;
using Serilog;

namespace Application.Features.Posts.Queries;

public record IsPostCreatorQuery(Guid UserId, Guid PostId) : IRequest<bool>;


public record IsPostCreatorQueryHandler(IReadPostRepository _repository) : IRequestHandler<IsPostCreatorQuery, bool>
{
    public async Task<bool> Handle(IsPostCreatorQuery request, CancellationToken cancellationToken)
    {
        try
        {
            bool isCreator = await _repository.IsUserPostCreator(request.UserId, request.PostId);
            return isCreator;
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return false;
        }
    }
}